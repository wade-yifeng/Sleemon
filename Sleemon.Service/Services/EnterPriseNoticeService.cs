namespace Sleemon.Service
{
    using System;
    using System.Linq;
    using System.Data.SqlClient;
    using System.Collections.Generic;

    using Microsoft.Practices.Unity;

    using Sleemon.Data;
    using Sleemon.Core;
    using Sleemon.Common;
    using System.Transactions;

    public class EnterpriseNoticeService : IEnterpriseNoticeService
    {
        private readonly ISleemonEntities _invoicingEntities;

        public EnterpriseNoticeService()
        {
            this._invoicingEntities = new SleemonEntities();
        }

        public IList<EnterpriseNoticePreviewModel> GetEnterpriseSummeryNotices(int pageIndex, int pageSize)
        {
            return this._invoicingEntities.Database.SqlQuery<EnterpriseNoticePreviewModel>(@"
WITH [NoticeWithRow] AS
(
     SELECT [EnterpriseNotice].[Id]
		   ,[EnterpriseNotice].[Subject]
		   ,[EnterpriseNotice].[Summary]
		   ,[EnterpriseNotice].[NoticeType]
		   ,[EnterpriseNotice].[Category]
		   ,[EnterpriseNotice].[AvatarPath]
		   ,[EnterpriseNotice].[LastUpdateTime]
		   ,ISNULL([UserComments].[CommentCount], 0) AS [CommentCount]
           ,CASE WHEN [Category].[IsTop] IS NULL THEN 0 ELSE 1 END AS [IsTop]
           ,ROW_NUMBER() OVER(ORDER BY [IsTop] DESC, [EnterpriseNotice].[LastUpdateTime] DESC, [EnterpriseNotice].[Id]) AS [RowNumber]
	FROM [dbo].[EnterpriseNotice]
	LEFT JOIN (SELECT [UserComments].[LinkedId], COUNT(*) AS [CommentCount] FROM [dbo].[UserComments] WHERE [UserComments].[IsActive] = 1 GROUP BY [UserComments].[LinkedId]) AS [UserComments]
		ON [UserComments].[LinkedId] = [EnterpriseNotice].[Id]
    LEFT JOIN (VALUES(1)) [Category]([IsTop])
        ON [Category].[IsTop] = [EnterpriseNotice].[Category]
	WHERE [EnterpriseNotice].[IsActive] = 1
)
SELECT [NoticeWithRow].[Id]
	  ,[NoticeWithRow].[Subject]
	  ,[NoticeWithRow].[Summary]
	  ,[NoticeWithRow].[NoticeType]
	  ,[NoticeWithRow].[Category]
	  ,[NoticeWithRow].[AvatarPath]
	  ,[NoticeWithRow].[LastUpdateTime]
	  ,[NoticeWithRow].[CommentCount]
FROM [NoticeWithRow]
WHERE [NoticeWithRow].[RowNumber] BETWEEN (@pageIndex - 1) * @pageSize + 1 AND @pageIndex * @pageSize",
                new SqlParameter("@pageIndex", pageIndex),
                new SqlParameter("@pageSize", pageSize)).ToList();
        }

        public IList<EnterpriseNoticePreviewModel> GetSlideEnterpriseSummeryNotices(int topCount)
        {
            return this._invoicingEntities.Database.SqlQuery<EnterpriseNoticePreviewModel>(@"
SELECT TOP(@top) [EnterpriseNotice].[Id]
		,[EnterpriseNotice].[Subject]
		,[EnterpriseNotice].[Summary]
		,[EnterpriseNotice].[NoticeType]
		,[EnterpriseNotice].[Category]
		,[EnterpriseNotice].[AvatarPath]
		,[EnterpriseNotice].[LastUpdateTime]
		,ISNULL([UserComments].[CommentCount], 0) AS [CommentCount]
FROM [dbo].[EnterpriseNotice]
LEFT JOIN (SELECT [UserComments].[LinkedId], COUNT(*) AS [CommentCount] FROM [dbo].[UserComments] WHERE [UserComments].[IsActive] = 1 GROUP BY [UserComments].[LinkedId]) AS [UserComments]
	ON [UserComments].[LinkedId] = [EnterpriseNotice].[Id]
WHERE [EnterpriseNotice].[IsActive] = 1
    AND [EnterpriseNotice].[Category] = 2",
               new SqlParameter("@top", topCount)).ToList();
        }

        public EnterpriseNoticeDetailModel GetEnterpriseNoticeById(int id)
        {
            var enterpriseNotice = this._invoicingEntities.Database.SqlQuery<EnterpriseNoticeDetailModel>(@"
SELECT [EnterpriseNotice].[Id]
	  ,[EnterpriseNotice].[Subject]
	  ,[EnterpriseNotice].[Category]
	  ,[EnterpriseNotice].[Summary]
	  ,[EnterpriseNotice].[AvatarPath]
	  ,[EnterpriseNotice].[Context]
	  ,[EnterpriseNotice].[NoticeType]
	  ,[EnterpriseNotice].[LastUpdateTime]
      ,ISNULL([Pros].[ProsCount], 0) AS [Pros]
      ,ISNULL([Cons].[ConsCount], 0) AS [Cons]
FROM [dbo].[EnterpriseNotice]
LEFT JOIN (SELECT [ProsEnterpriseNotice].[EnterpriseNoticeId], COUNT(*) AS [ProsCount] FROM [dbo].[ProsEnterpriseNotice] GROUP BY [ProsEnterpriseNotice].[EnterpriseNoticeId]) AS [Pros]
    ON [Pros].[EnterpriseNoticeId] = [EnterpriseNotice].[Id]
LEFT JOIN (SELECT [ConsEnterpriseNotice].[EnterpriseNoticeId], COUNT(*) AS [ConsCount] FROM [dbo].[ConsEnterpriseNotice] GROUP BY [ConsEnterpriseNotice].[EnterpriseNoticeId]) AS [Cons]
    ON [Cons].[EnterpriseNoticeId] = [EnterpriseNotice].[Id]
WHERE [EnterpriseNotice].[IsActive] = 1
    AND [EnterpriseNotice].[Id] = @id", new SqlParameter("@id", id)).FirstOrDefault();

            return enterpriseNotice;
        }

        public ProConNoticeResult ProNoticeById(int id, string userId)
        {
            var result = new ProConNoticeResult()
            {
                NoticeId = id,
                UserId = userId,
                IsPro = true,
                IsSuccess = true
            };

            var prosEntity =
                this._invoicingEntities.ProsEnterpriseNotice.FirstOrDefault(
                    p => p.EnterpriseNoticeId == id && p.UserUniqueId == userId);
            var consEntity =
                this._invoicingEntities.ConsEnterpriseNotice.FirstOrDefault(
                    p => p.EnterpriseNoticeId == id && p.UserUniqueId == userId);

            if (prosEntity == null)
            {
                prosEntity = this._invoicingEntities.ProsEnterpriseNotice.Create();

                prosEntity.EnterpriseNoticeId = id;
                prosEntity.UserUniqueId = userId;
                prosEntity.ProsDateTime = DateTime.UtcNow;

                result.StatusCode = (int)StatusCode.ProsSuccess;

                this._invoicingEntities.ProsEnterpriseNotice.Add(prosEntity);
            }
            else
            {
                result.IsPro = false;

                result.StatusCode = (int)StatusCode.CancelProsSuccess;

                this._invoicingEntities.ProsEnterpriseNotice.Remove(prosEntity);
            }

            if (consEntity != null)
            {
                this._invoicingEntities.ConsEnterpriseNotice.Remove(consEntity);
            }

            this._invoicingEntities.SaveChanges();

            result.Cons = this._invoicingEntities.ConsEnterpriseNotice.Count(p => p.EnterpriseNoticeId == id);
            result.Pros = this._invoicingEntities.ProsEnterpriseNotice.Count(p => p.EnterpriseNoticeId == id);

            return result;
        }

        public ProConNoticeResult ConNoticeById(int id, string userId)
        {
            var result = new ProConNoticeResult()
            {
                NoticeId = id,
                UserId = userId,
                IsCon = true,
                IsSuccess = true
            };

            var consEntity =
                this._invoicingEntities.ConsEnterpriseNotice.FirstOrDefault(
                    p => p.EnterpriseNoticeId == id && p.UserUniqueId == userId);
            var prosEntity =
                this._invoicingEntities.ProsEnterpriseNotice.FirstOrDefault(
                    p => p.EnterpriseNoticeId == id && p.UserUniqueId == userId);

            if (consEntity == null)
            {
                consEntity = this._invoicingEntities.ConsEnterpriseNotice.Create();

                consEntity.EnterpriseNoticeId = id;
                consEntity.UserUniqueId = userId;
                consEntity.ConsDateTime = DateTime.UtcNow;

                result.StatusCode = (int)StatusCode.ConsSuccess;

                this._invoicingEntities.ConsEnterpriseNotice.Add(consEntity);
            }
            else
            {
                result.IsCon = false;

                result.StatusCode = (int)StatusCode.CancelConsSuccess;

                this._invoicingEntities.ConsEnterpriseNotice.Remove(consEntity);
            }

            if (prosEntity != null)
            {
                this._invoicingEntities.ProsEnterpriseNotice.Remove(prosEntity);
            }

            this._invoicingEntities.SaveChanges();

            result.Cons = this._invoicingEntities.ConsEnterpriseNotice.Count(p => p.EnterpriseNoticeId == id);
            result.Pros = this._invoicingEntities.ProsEnterpriseNotice.Count(p => p.EnterpriseNoticeId == id);

            return result;
        }
        
        /// <summary>
        /// add by wolfgump 20160522
        /// 提交企业资讯
        /// </summary>
        /// <param name="enModel"></param>
        /// <returns></returns>
        public ResultBase SubmitEnterpriseNotice(EnterpriseNoticeSubmitModel enpwModel)
        {
            //插入EnterpriseNotice表
            EnterpriseNotice enModel = this._invoicingEntities.EnterpriseNotice.Create();
            enModel = EmitHelper.ConvertTo<EnterpriseNoticeSubmitModel, EnterpriseNotice>(enpwModel);
            this._invoicingEntities.EnterpriseNotice.Add(enModel);
            int res = this._invoicingEntities.SaveChanges();

            return new ResultBase()
            {
                IsSuccess = res > 0,
                StatusCode = res > 0 ? (int)StatusCode.Success : (int)StatusCode.Failed
            };

            ////如果需要推送 插入MessageDispatch表
            ////两张表同时写入成功  使用事务
            //using (TransactionScope tran = new TransactionScope())
            //{
            //    //插入EnterpriseNotice表
            //    EnterpriseNotice enModel = this._invoicingEntities.EnterpriseNotice.Create();
            //    enModel = EmitHelper.ConvertTo<EnterpriseNoticeSubmitModel, EnterpriseNotice>(enpwModel);
            //    this._invoicingEntities.EnterpriseNotice.Add(enModel);
            //    this._invoicingEntities.SaveChanges();//savechange 为了拿到新的id
            //    int enId = this._invoicingEntities.EnterpriseNotice.Local[0].Id;

            //    string departmentHierarchyPath = enpwModel.DepartmentHierarchyPath;
            //    string departmentUniqueid = enpwModel.DepartmentUniqueId;
            //    if (!string.IsNullOrEmpty(departmentHierarchyPath) && !string.IsNullOrEmpty(departmentUniqueid))
            //    {
            //        //插入DepartmentEnterpriseNotice表中
            //        string[] departmentHierarchyPathArray = departmentHierarchyPath.Split('|');
            //        for (int i = 0; i < departmentHierarchyPathArray.Length; i++)
            //        {
            //            DepartmentEnterpriseNotice denModel = this._invoicingEntities.DepartmentEnterpriseNotice.Create();
            //            denModel.EnterpriseNoticeId = enId;
            //            denModel.DepartmentHierarchyPath = departmentHierarchyPathArray[i];
            //            this._invoicingEntities.DepartmentEnterpriseNotice.Add(denModel);
            //        }
            //        //插入MessageDispatch
            //        MessageDispatch messageModel = this._invoicingEntities.MessageDispatch.Create();
            //        messageModel.Subject = enpwModel.Subject;
            //        messageModel.ToDepts = departmentUniqueid;
            //        messageModel.MessageType = enpwModel.NoticeType;
            //        messageModel.LinkedId = enId;
            //        messageModel.DispatchType = 1;//todo Dispatchtime
            //        messageModel.IsActive = true;
            //        messageModel.LastUpdateUser = 123;//todo
            //        messageModel.LastUpdateTime = DateTime.Now;
            //        this._invoicingEntities.MessageDispatch.Add(messageModel);
            //    }

            //    int res = this._invoicingEntities.SaveChanges();
            //    tran.Complete();
            //    return res > 0;
            //}
        }

        public IList<EnterpriseNotice> GetEnterpriseNotices(int pageIndex, int pageSize, string noticeTitle, out int totalCount)
        {
            totalCount = 0;
            var list = from r in this._invoicingEntities.EnterpriseNotice
                       where r.IsActive == true
                       select r;
            if (!string.IsNullOrEmpty(noticeTitle))
            {
                list = list.Where(p => p.Subject.Contains(noticeTitle));
            }
            if (list == null)
            {
                return null;
            }
            totalCount = list.Count();
            list = list.OrderByDescending(p => p.LastUpdateTime).Take(pageSize * pageIndex).Skip(pageSize * (pageIndex - 1));
            return list.ToList();
        }

        public IList<EnterpriseNoticePreviewModel> GetLatestNotices(int previousLatestNoticeId)
        {
            return this._invoicingEntities.Database.SqlQuery<EnterpriseNoticePreviewModel>(@"
DECLARE @latestUpdatedTime DATETIME = (SELECT [EnterpriseNotice].[LastUpdateTime] FROM [dbo].[EnterpriseNotice] WHERE [EnterpriseNotice].[IsActive] = 1 AND [EnterpriseNotice].[Id] = @previousLatestNoticeId);

SELECT [EnterpriseNotice].[Id]
	  ,[EnterpriseNotice].[Subject]
	  ,[EnterpriseNotice].[Summary]
	  ,[EnterpriseNotice].[NoticeType]
	  ,[EnterpriseNotice].[Category]
      ,[EnterpriseNotice].[AvatarPath]
	  ,ISNULL([UserComments].[CommentCount], 0) AS [CommentCount]
      ,[EnterpriseNotice].[LastUpdateTime]
FROM [dbo].[EnterpriseNotice]
LEFT JOIN (SELECT [UserComments].[LinkedId], COUNT(*) AS [CommentCount] FROM [dbo].[UserComments] WHERE [UserComments].[IsActive] = 1 GROUP BY [UserComments].[LinkedId]) AS [UserComments]
	ON [UserComments].[LinkedId] = [EnterpriseNotice].[Id]
WHERE [EnterpriseNotice].[IsActive] = 1
	AND [EnterpriseNotice].[LastUpdateTime] > @latestUpdatedTime", new SqlParameter("@previousLatestNoticeId", previousLatestNoticeId)).ToList();
        }
    }
}

