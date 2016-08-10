using Sleemon.Common;
using Sleemon.Core;
using Sleemon.Data;
using Sleemon.Portal.Common;
using Sleemon.Portal.Core;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sleemon.Portal.Controllers
{
    public class TrainingController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.UserUniqueId = User.GetUserUniqueId();
            return View("TrainingList");
        }

        [HttpGet]
        public ActionResult Detail()
        {
            return View("TrainingDetail");
        }

        [HttpGet]
        public JsonContentAction GetTrainingList(TrainingSearchContext search)
        {
            ViewBag.pageIndex = search.PageIndex;
            ViewBag.CurrentUserId = UserUniqueId;

            var trainings = ServiceClient.Request<ITrainingService, IList<TrainingListModel>>(
                service => service.GetTrainingList(search));

            return new JsonContentAction(trainings);
        }

        [HttpGet]
        public JsonContentAction GetTrainingDetail(int id)
        {
            var training = ServiceClient.Request<ITrainingService, TrainingDetailModel>(
                service => service.GetTrainingDetailById(id));

            return new JsonContentAction(training);
        }

        public ActionResult Detail(int id)
        {
            var training = ServiceClient.Request<ITrainingService, TrainingDetailModel>(
                service => service.GetTrainingDetailById(id));

            return PartialView("TrainingDetail", training);
        }

        [HttpGet]
        public ActionResult TrainingCreate(int id = 0)
        {
            var training = id == 0
                ? new TrainingDetailModel()
                : ServiceClient.Request<ITrainingService, TrainingDetailModel>(
                    service => service.GetTrainingDetailById(id));

            return PartialView("TrainingCreate", training);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult TrainingCreate(TrainingDetailModel training)
        {
            var msg = "Save successful.";
            var validation = ValidateModelForTraining(training);
            if (string.IsNullOrEmpty(validation))
            {
                EnrichTrainingDetailModel(training);
                var result = ServiceClient.Request<ITrainingService, ResultBase>(
                    service => service.SaveTrainingDetail(training));
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
        public ActionResult DeleteTraining(int id)
        {
            var msg = "Delete successful.";
            var result = ServiceClient.Request<ITrainingService, ResultBase>(
                    service => service.DeleteTrainingById(id));
            if (!result.IsSuccess)
            {
                msg = result.Message;
            }

            return new JsonResult() { Data = msg };
        }

        [HttpGet]
        public ActionResult ConfirmUserJoin(int id)
        {
            var users = ServiceClient.Request<ITrainingService, IList<UserViewModel>>(
                service => service.GetTrainingParticipants(id));

            return PartialView("ConfirmUserJoin", users);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ConfirmUserJoin(int id, string confirmedUsers, string rejectedUsers )
        {
            var msg = "Confirm successful.";
            var training = ServiceClient.Request<ITrainingService, TrainingDetailModel>(
                    service => service.GetTrainingDetailById(id));
            if (training != null && training.LastUpdateUser == UserUniqueId)
            {
                if (training.Status == (byte)ActionCategory.Publish)
                {
                    var joinStatusUser = new Dictionary<JoinStatus, IList<string>>
                    {
                        {JoinStatus.Approved, confirmedUsers.Split(';').ToList()},
                        {JoinStatus.Rejected, rejectedUsers.Split(';').ToList()}
                    };

                    var result = ServiceClient.Request<ITrainingService, ResultBase>(
                        service => service.UpdateTrainingUsersJoinState(id, joinStatusUser));
                    if (!result.IsSuccess)
                    {
                        msg = result.Message;
                    }
                }
                else
                {
                    msg = @"Cannot confirm again.";
                }
            }
            else
            {
                msg = @"Training info is not found.";
            }

            return new JsonResult() { Data = msg };
        }

        public JsonResult UpLoadImageFile()
        {
            var model = new UpLoadFileModel();
            var result = new ResultBase();

            if (Request.Files.Count <= 0)
            {
                result.StatusCode = -1;
                result.Message = "请选择要上传的图片";
                model.Result = result;
            }
            else
            {
                string message;
                var file = Request.Files[0];

                if (Utilities.UpLoadImageFile(file, Server, out message))
                {
                    result.StatusCode = 0;
                    result.Message = "上传成功";
                    model.Result = result;
                    model.filePath = message;
                }
                else
                {
                    result.StatusCode = -1;
                    result.Message = message;
                    model.Result = result;
                }
            }
            return new JsonResult() { Data = model };
        }

        public ActionResult GetTaskItemList(TaskItemSearchContext search)
        {
            var itemList = new Dictionary<int, string>();

            switch (search.TaskCategory)
            {
                case (byte)TaskCategory.Learning:
                    itemList = ServiceClient.Request<ILearningFileService, IList<LearningFileListModel>>(
                        service => service.GetLearningFileList(search.PageIndex, search.PageSize, search.Title))
                        .ToDictionary(u => u.Id, u => u.Subject);
                    break;
                case (byte)TaskCategory.Exam:
                    itemList = ServiceClient.Request<IExamService, IList<ExamListModel>>(
                        service => service.GetExamList())
                        .ToDictionary(u => u.Id, u => u.Title);
                    break;
                case (byte)TaskCategory.Questionnaire:
                    itemList = ServiceClient.Request<IQuestionnaireService, IList<QuestionnaireListModel>>(
                        service => service.GetQuestionnaireList(search.PageIndex, search.PageSize, search.Title))
                        .ToDictionary(u => u.Id, u => u.Title);
                    break;
            }

            ViewBag.TaskCategory = search.TaskCategory;
            ViewBag.PageIndex = search.PageIndex;
            
            return PartialView("TaskItemList", itemList);
        }

        private void EnrichTrainingDetailModel(TrainingDetailModel training)
        {
            training.LastUpdateUser = UserUniqueId;

            if (training.Status == (byte)ActionCategory.Publish)
            {
                var tasks = new List<TaskDetailsModel>();
                // create checkin task behind the scene.
                if (training.IsCheckInNeeded)
                {
                    var task = new TaskDetailsModel
                    {
                        TaskCategory = (byte)TaskCategory.CheckIn,
                        Title = string.Format(training.Subject + "签到"),
                        Point = 20, //TODO: retrieve from config
                        ExhibitAbility = 0,
                        ProductAbility = 0,
                        SalesAbility = 0
                    };
                    training.Tasks.Insert(0, task);
                    tasks.Add(task);
                }

                foreach (var task in training.Tasks)
                {
                    task.Status = (byte)training.Status;
                    task.Description = task.Title;
                    task.OverduePoint = task.Point;
                    task.BelongTo = (int)TaskBelongTo.TrainingTask;
                    task.LastUpdateUser = UserUniqueId;
                    task.StartFrom = training.StartFrom;
                    task.EndTo = training.EndTo;
                }

                // set the order of training tasks by checkin, learning, exam, questionnaire.
                var learningTask = training.Tasks.Where(u => u.TaskCategory == (byte)TaskCategory.Learning);
                var examTask = training.Tasks.Where(u => u.TaskCategory == (byte) TaskCategory.Exam);
                var questionnaireTask = training.Tasks.Where(u => u.TaskCategory == (byte) TaskCategory.Questionnaire);
                tasks.AddRange(learningTask);
                tasks.AddRange(examTask);
                tasks.AddRange(questionnaireTask);

                training.Tasks = tasks;
            }
        }

        private string ValidateModelForTraining(TrainingDetailModel training)
        {
            return string.Empty;
        }       
    }
}