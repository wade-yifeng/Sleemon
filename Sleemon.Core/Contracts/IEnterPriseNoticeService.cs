namespace Sleemon.Core
{
    using System.Collections.Generic;

    using Sleemon.Data;

    public interface IEnterpriseNoticeService
    {
        IList<EnterpriseNoticePreviewModel> GetEnterpriseSummeryNotices(int pageIndex, int pageSize);

        IList<EnterpriseNoticePreviewModel> GetSlideEnterpriseSummeryNotices(int topCount);

        EnterpriseNoticeDetailModel GetEnterpriseNoticeById(int id);

        ProConNoticeResult ProNoticeById(int id, string userId);

        ProConNoticeResult ConNoticeById(int id, string userId);

        ResultBase SubmitEnterpriseNotice(EnterpriseNoticeSubmitModel enModel);

        IList<EnterpriseNotice> GetEnterpriseNotices(int pageIndex, int pageSize, string noticeTitle, out int totalCount);

        IList<EnterpriseNoticePreviewModel> GetLatestNotices(int previousLatestNoticeId);

    }
}
