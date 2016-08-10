using Sleemon.Common;
using Sleemon.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sleemon.Core
{
    public interface ICommentService
    {
        NewCommentViewModel AddNewComment(int linkedId, string userId, string comment, byte category);

        IList<UserComment> GetTopCommentsByLinkedId(byte category, int linkedId, string userId, int topCount);

        IList<UserComment> GetPagedCommentsByLinkedId(byte category, int linkedId, string userId, int pageIndex, int pageSize, out int totalCount);

        ProCommentResult ProCommentById(int id, string userId);

        IList<UserComment> GetUserCommentsByStatus(LegalStatus legalStatus);

        ResultBase UpdateCommentLegalStatus(int commentId, byte legalStatus);
    }
}
