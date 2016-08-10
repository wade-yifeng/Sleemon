using Sleemon.Common;
using Sleemon.Core;
using Sleemon.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sleemon.Service
{
    public class CommentService : ICommentService
    {
        private readonly ISleemonEntities _invoicingEntities;

        public CommentService()
        {
            this._invoicingEntities = new SleemonEntities();
        }

        public NewCommentViewModel AddNewComment(int linkedId, string userId, string comment, byte category)
        {
            var result = new NewCommentViewModel()
            {
                IsSuccess = true,
                StatusCode = (int)StatusCode.Success
            };

            var entity = this._invoicingEntities.UserComments.Create();

            if (entity != null)
            {
                entity.LinkedId = linkedId;
                entity.UserUniqueId = userId;
                entity.Comment = comment;
                entity.Category = category;
                entity.LegalStatus = (byte)LegalStatus.UnCheck; //TODO: Check this comment is legal or not
                entity.CommentTime = DateTime.UtcNow;
                entity.IsActive = true;

                this._invoicingEntities.UserComments.Add(entity);
                this._invoicingEntities.SaveChanges();
            }
            else
            {
                throw new Exception("Create User Comment entity failed");
            }

            var userEntity = this._invoicingEntities.User.FirstOrDefault(p => p.IsActive && p.UserUniqueId == userId);

            result.NewComment = new UserComment()
            {
                Id = entity.Id,
                Avatar = userEntity != null ? userEntity.Avatar : string.Empty,
                Name = userEntity != null ? userEntity.Name : string.Empty,
                Comment = entity.Comment,
                CommentTime = entity.CommentTime
            };

            return result;
        }

        public IList<UserComment> GetTopCommentsByLinkedId(byte category, int linkedId, string userId, int topCount)
        {
            return this._invoicingEntities.Database.SqlQuery<UserComment>(@"
SELECT [UserComments].[Id]
	  ,[User].[Avatar]
	  ,[User].[Name]
	  ,[UserComments].[Comment]
	  ,ISNULL([Pros].[ProsCount], 0)	AS [Pros]
	  ,[UserComments].[CommentTime]
      ,CAST(CASE WHEN [MyComments].[UserCommentId] IS NULL THEN 0 ELSE 1 END AS BIT)                 AS [IsProByMe]
FROM [dbo].[UserComments]
JOIN [dbo].[User]
	ON [User].[UserUniqueId] = [UserComments].[UserUniqueId]
        AND [User].[IsActive] = 1
JOIN (SELECT [ProsComments].[UserCommentId], COUNT(*) AS [ProsCount] FROM [dbo].[ProsComments] GROUP BY [ProsComments].[UserCommentId]) AS [Pros]
	ON [Pros].[UserCommentId] = [UserComments].[Id]
LEFT JOIN (SELECT DISTINCT [ProsComments].[UserCommentId] FROM [dbo].[ProsComments] WHERE [ProsComments].[UserUniqueId] = @userId) AS [MyComments]
    ON [MyComments].[UserCommentId] = [UserComments].[Id]
WHERE [UserComments].[LinkedId] = @linkedId
    AND [UserComments].[Category] = @category
ORDER BY [Pros].[ProsCount] DESC, [UserComments].[CommentTime] DESC", 
                new SqlParameter("@linkedId", linkedId),
                new SqlParameter("@category", category), 
                new SqlParameter("@userId", userId))
                .Take(topCount)
                .ToList();
        }

        public IList<UserComment> GetPagedCommentsByLinkedId(byte category, int linkedId, string userId, int pageIndex, int pageSize, out int totalCount)
        {
            totalCount = 0;

            var result = this._invoicingEntities.Database.SqlQuery<PagedUserComment>(@"
WITH [UserComment] AS
(
	SELECT [UserComments].[Id]
		  ,[User].[Avatar]	AS [Avatar]
		  ,[User].[Name]
		  ,[UserComments].[Comment]
		  ,ISNULL([Pros].[ProsCount], 0)			AS [Pros]
		  ,[UserComments].[CommentTime]
          ,CAST(CASE WHEN [MyComments].[UserCommentId] IS NULL THEN 0 ELSE 1 END AS BIT)                 AS [IsProByMe]
		  ,ROW_NUMBER() OVER(ORDER BY [UserComments].[CommentTime] DESC) AS [RowNumber]
		  ,COUNT(*) OVER()												 AS [TotalCount]
	FROM [dbo].[UserComments]
	JOIN [dbo].[User]
		ON [User].[UserUniqueId] = [UserComments].[UserUniqueId]
            AND [User].[IsActive] = 1
	LEFT JOIN (SELECT [ProsComments].[UserCommentId], COUNT(*) AS [ProsCount] FROM [dbo].[ProsComments] GROUP BY [ProsComments].[UserCommentId]) AS [Pros]
		ON [Pros].[UserCommentId] = [UserComments].[Id]
    LEFT JOIN (SELECT DISTINCT [ProsComments].[UserCommentId] FROM [dbo].[ProsComments] WHERE [ProsComments].[UserUniqueId] = @userId) AS [MyComments]
        ON [MyComments].[UserCommentId] = [UserComments].[Id]
	WHERE [UserComments].[LinkedId] = @linkedId
        AND [UserComments].[Category] = @category
)
SELECT [UserComment].[Id]
	  ,[UserComment].[Avatar]
	  ,[UserComment].[Name]
	  ,[UserComment].[Comment]
	  ,[UserComment].[Pros]
	  ,[UserComment].[CommentTime]
      ,[UserComment].[IsProByMe]
	  ,[UserComment].[TotalCount]
FROM [UserComment]
WHERE [UserComment].[RowNumber] BETWEEN (@pageIndex - 1) * @pageSize + 1 AND @pageIndex * @pageSize",
                new SqlParameter("@linkedId", linkedId),
                new SqlParameter("@userId", userId),
                new SqlParameter("@category", category), 
                new SqlParameter("@pageIndex", pageIndex),
                new SqlParameter("@pageSize", pageSize)).ToList();

            var firstResult = result.FirstOrDefault();

            if (firstResult != null) { totalCount = firstResult.TotalCount; }

            return result.Select(p => new UserComment()
            {
                Id = p.Id,
                Avatar = p.Avatar,
                Name = p.Name,
                Comment = p.Comment,
                Pros = p.Pros,
                CommentTime = p.CommentTime,
                IsProByMe = p.IsProByMe
            }).ToList();
        }

        public ProCommentResult ProCommentById(int id, string userId)
        {
            var result = new ProCommentResult()
            {
                CommentId = id,
                UserId = userId,
                IsPro = true,
                IsSuccess = true
            };

            var entity =
                this._invoicingEntities.ProsComments.FirstOrDefault(
                    p => p.UserCommentId == id && p.UserUniqueId == userId);

            if (entity == null)
            {
                entity = this._invoicingEntities.ProsComments.Create();

                entity.UserCommentId = id;
                entity.UserUniqueId = userId;
                entity.ProsDateTime = DateTime.UtcNow;

                result.StatusCode = (int)StatusCode.ProsSuccess;

                this._invoicingEntities.ProsComments.Add(entity);
            }
            else
            {
                result.IsPro = false;

                result.StatusCode = (int)StatusCode.CancelProsSuccess;

                this._invoicingEntities.ProsComments.Remove(entity);
            }

            this._invoicingEntities.SaveChanges();

            result.Pros = this._invoicingEntities.ProsComments.Count(p => p.UserCommentId == id);

            return result;
        }

        public ResultBase UpdateCommentLegalStatus(int commentId, byte legalStatus)
        {
            var result = new ResultBase()
            {
                IsSuccess = true,
                StatusCode = (int)StatusCode.Success
            };

            var entity =
                this._invoicingEntities.UserComments.FirstOrDefault(
                    p => p.Id == commentId && p.IsActive);

            if (entity != null)
            {
                entity.LegalStatus = legalStatus;
                this._invoicingEntities.SaveChanges();
            }
            else
            {
                result.IsSuccess = false;
                result.StatusCode = (int)StatusCode.Failed;
                result.Message = string.Format("Not found comment, @id: {0}", commentId);
            }

            return result;
        }

        public IList<UserComment> GetUserCommentsByStatus(LegalStatus legalStatus)
        {
            var comments = new List<UserComment>();

            if (legalStatus == LegalStatus.Illegal)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("DECLARE @Words VARCHAR(8000)");
                sb.Append(" SELECT @Words = COALESCE(@Words + ' OR ', '') + '\"' + IllegalWord + '\"'");
                sb.Append(" FROM [dbo].[IllegalCharacters]	WHERE IsActive = 1");
                sb.Append(" SET @Words = '''' + @Words + ''''");
                sb.Append(" SELECT  [UserComments].[Id], [Name], [Comment], [Category], [LegalStatus], [CommentTime]");
                sb.Append(" FROM [dbo].[UserComments]");
                sb.Append(" LEFT JOIN [dbo].[User]");
                sb.Append(" ON [User].[UserUniqueId] = [UserComments].[UserUniqueId] AND [User].[IsActive] = 1");
                sb.Append(" WHERE [UserComments].[IsActive] = 1 AND [LegalStatus] = 0");
                sb.Append("AND FREETEXT(Comment, @Words)");
                //_invoicingEntities.spGetIllegalComments();
                return this._invoicingEntities.Database.SqlQuery<UserComment>(sb.ToString()).ToList();
            }

            var userEntities = this._invoicingEntities.User.Where(p => p.IsActive).ToList();
            var commentEntities = this._invoicingEntities.UserComments.Where(u => u.IsActive && u.LegalStatus == (byte)legalStatus).ToList();

            return commentEntities
                .Select(u =>
                {
                    var lastUpdateUserName = string.Empty;
                    var firstOrDefault = userEntities.FirstOrDefault(o => o.UserUniqueId == u.UserUniqueId);
                    if (firstOrDefault != null)
                    {
                        lastUpdateUserName = firstOrDefault.Name;
                    }

                    return new UserComment()
                    {
                        Id = u.Id,
                        Name = lastUpdateUserName,
                        Comment = u.Comment,
                        Category = u.Category,
                        LegalStatus = u.LegalStatus,
                        CommentTime = u.CommentTime
                    };

                }).ToList();
        }

    }
}
