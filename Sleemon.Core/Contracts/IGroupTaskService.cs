namespace Sleemon.Core
{
    using System.Collections.Generic;
    using Sleemon.Data;

    public interface IGroupTaskService
    {
        IList<GroupTaskListModel> GetGroupTaskList(GroupTaskSearchContext search);

        GroupTaskDetailModel GetGroupTaskDetailById(int groupTaskId);

        ResultBase SaveGroupTaskDetail(GroupTaskDetailModel groupTask);

        ResultBase DeleteGroupTaskById(int groupTaskId);

        ResultBase SwitchGroupTaskStatus(int groupTaskId, int onOff);
    }
}
