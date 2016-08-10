using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Sleemon.Common;
using Sleemon.Data;
using Sleemon.Portal.Common;
using Sleemon.Portal.Core;
using Sleemon.Core;

namespace Sleemon.Portal.Controllers
{
    public class GroupTaskController : BaseController
    {
        [HttpGet]
        public ActionResult Index(GroupTaskSearchContext search)
        {
            ViewBag.pageIndex = search.PageIndex;
            ViewBag.CurrentUserId = UserUniqueId;

            var groupTasks =
                ServiceClient.Request<IGroupTaskService, IList<GroupTaskListModel>>(
                    service => service.GetGroupTaskList(search));

            return PartialView("GroupTaskList", groupTasks);
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            var groupTasks =
                ServiceClient.Request<IGroupTaskService, GroupTaskDetailModel>(
                    service => service.GetGroupTaskDetailById(id));

            return PartialView("GroupTaskDetail", groupTasks);
        }

        [HttpGet]
        public ActionResult GroupTaskCreate(int id = 0)
        {
            var groupTasks = id == 0
                ? new GroupTaskDetailModel()
                : ServiceClient.Request<IGroupTaskService, GroupTaskDetailModel>(
                    service => service.GetGroupTaskDetailById(id));

            return PartialView("GroupTaskCreate", groupTasks);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GroupTaskCreate(GroupTaskDetailModel groupTask)
        {
            var msg = "Save successful.";
            var validation = ValidateModelForGroupTask(groupTask);
            if (string.IsNullOrEmpty(validation))
            {
                EnrichGroupTaskDetailModel(groupTask);
                var result =
                    ServiceClient.Request<IGroupTaskService, ResultBase>(
                        service => service.SaveGroupTaskDetail(groupTask));

                if (!result.IsSuccess)
                {
                    msg = result.Message;
                }
            }
            else
            {
                msg = validation;
            }

            return new JsonResult() { Data = msg };
        }
        
        [HttpPost, ValidateInput(false)]
        public ActionResult DeleteGroupTask(int id)
        {
            var msg = "Delete successful.";
            var result = 
                ServiceClient.Request<IGroupTaskService, ResultBase>(
                        service => service.DeleteGroupTaskById(id));
            if (!result.IsSuccess)
            {
                msg = result.Message;
            }

            return new JsonResult() { Data = msg };
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SwitchStatus(int groupTaskId, int onOff)
        {
            var msg = "successful.";
            var result =
                ServiceClient.Request<IGroupTaskService, ResultBase>(
                    service => service.SwitchGroupTaskStatus(groupTaskId, onOff));
            if (!result.IsSuccess)
            {
                msg = result.Message;
            }

            return new JsonResult() { Data = msg };
        }

        private string ValidateModelForGroupTask(GroupTaskDetailModel groupTasks)
        {
            return string.Empty;
        }

        private void EnrichGroupTaskDetailModel(GroupTaskDetailModel groupTasks)
        {
            groupTasks.LastUpdateUser = UserUniqueId;

            if (groupTasks.Status == (byte)ActionCategory.Publish)
            {
                foreach (var subTask in groupTasks.SubTasks)
                {
                    subTask.StartFrom = DateTime.Now;
                    subTask.Status = groupTasks.Status;
                    subTask.BelongTo = (byte)TaskBelongTo.GroupTask;
                    subTask.LastUpdateUser = UserUniqueId;
                }
            }
        }
    }
}