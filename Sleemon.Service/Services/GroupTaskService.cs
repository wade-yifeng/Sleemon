namespace Sleemon.Service
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.Unity;
    using Sleemon.Core;
    using Sleemon.Data;

    public class GroupTaskService : IGroupTaskService
    {
        private readonly ISleemonEntities _invoicingEntities;

        public GroupTaskService()
        {
            this._invoicingEntities = new SleemonEntities();
        }

        public IList<GroupTaskListModel> GetGroupTaskList(GroupTaskSearchContext search)
        {
            throw new NotImplementedException();
        }

        public GroupTaskDetailModel GetGroupTaskDetailById(int groupTaskId)
        {
            throw new NotImplementedException();
        }

        public ResultBase SaveGroupTaskDetail(GroupTaskDetailModel groupTask)
        {
            throw new NotImplementedException();
        }

        public ResultBase DeleteGroupTaskById(int groupTaskId)
        {
            throw new NotImplementedException();
        }

        public ResultBase SwitchGroupTaskStatus(int groupTaskId, int onOff)
        {
            throw new NotImplementedException();
        }
    }
}
