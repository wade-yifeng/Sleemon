using System.Collections.Generic;
using System.Linq;
using Sleemon.Common;
using Sleemon.Core;
using Sleemon.Portal.Common;

namespace Sleemon.Portal.Controllers
{
    using System.Web.Mvc;
    using Microsoft.Practices.Unity;
    using Sleemon.Data;
    using Sleemon.Portal.Core;

    public class TaskController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.UserUniqueId = User.GetUserUniqueId();

            return PartialView("TaskList");
        }

        public JsonContentAction GetTaskList()
        {
            var tasks = ServiceClient.Request<ITaskService, IList<TaskListModel>>(
                service => service.GetTaskList((int) TaskBelongTo.SingleTask));

            return new JsonContentAction(tasks);
        }

        [HttpGet]
        public ActionResult Detail()
        {
            return View("TaskDetail");
        }

        [HttpGet]
        public JsonContentAction GetTaskDetail(int taskId)
        {
            var task = ServiceClient.Request<ITaskService, TaskDetailsModel>(
                    service => service.GetTaskDetailById(taskId));

            return new JsonContentAction(task);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View("TaskCreate");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TaskCreate(TaskDetailsModel task)
        {
            var msg = "Save successful.";
            var validation = ValidateModelForTask(task);
            if (string.IsNullOrEmpty(validation))
            {
                EnrichTaskDetailModel(task);
                var result = ServiceClient.Request<ITaskService, ResultBase>(
                    service => service.SaveTaskDetail(task));
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
        public ActionResult DeleteTask(int id)
        {
            var msg = "Delete successful.";
            var task = ServiceClient.Request<ITaskService, TaskDetailsModel>(
                    service => service.GetTaskDetailById(id));
            if (task != null && task.Status == (byte)ActionCategory.Save &&
                task.LastUpdateUser == UserUniqueId)
            {
                var result = ServiceClient.Request<ITaskService, ResultBase>(
                    service => service.DeleteTaskById(id));
                if (!result.IsSuccess)
                {
                    msg = result.Message;
                }
            }
            else
            {
                msg = @"Task info is not found.";
            }

            return new JsonResult() { Data = msg };
        }

        public JsonContentAction GetTaskItemList(int category)
        {
            var itemList = new List<TaskItemModel>();

            switch (category)
            {
                case (byte)TaskCategory.Learning:
                    itemList =
                        ServiceClient.Request<ILearningFileService, IList<LearningFileListModel>>(
                            service => service.GetLearningFileList())
                            .Select(u => new TaskItemModel {Id = u.Id, Title = u.Subject})
                            .ToList();
                    break;
                case (byte)TaskCategory.Exam:
                    itemList =
                        ServiceClient.Request<IExamService, IList<ExamListModel>>(service => service.GetExamList())
                            .Select(u => new TaskItemModel {Id = u.Id, Title = u.Title})
                            .ToList();
                    break;
                case (byte)TaskCategory.Questionnaire:
                    itemList =
                        ServiceClient.Request<IQuestionnaireService, IList<QuestionnaireListModel>>(
                            service => service.GetQuestionnaireList())
                            .Select(u => new TaskItemModel {Id = u.Id, Title = u.Title})
                            .ToList();
                    break;
            }

           return new JsonContentAction(itemList);
        }
        
        private void EnrichTaskDetailModel(TaskDetailsModel task)
        {
            task.LastUpdateUser = UserUniqueId;
            task.BelongTo = (int)TaskBelongTo.SingleTask;

            if (task.Status == (byte)ActionCategory.Publish)
            {
                task.DispatchSubject = task.Title;
                task.DispatchType = (byte)MsgDispatchType.Immediate;
                task.DispatchPriority = 1;

                if (task.Exams!=null)
                {
                    foreach (var exam in task.Exams)
                    {
                        exam.LastUpdateUser = UserUniqueId;
                    }
                }

                if (task.LearningFiles != null)
                {
                    foreach (var learningFile in task.LearningFiles)
                    {
                        learningFile.LastUpdateUser = UserUniqueId;
                    }
                }

                if (task.Questionnaires != null)
                {
                    foreach (var questionnaire in task.Questionnaires)
                    {
                        questionnaire.LastUpdateUser = UserUniqueId;
                    }
                }
            }
        }

        private string ValidateModelForTask(TaskDetailsModel task)
        {
            return string.Empty;
        }
    }
}