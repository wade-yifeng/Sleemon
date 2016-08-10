using System.Collections.Generic;
using Sleemon.Core;

namespace Sleemon.Portal.Controllers
{
    using System.Web.Mvc;
    using System.Linq;
    using Microsoft.Practices.Unity;
    using Microsoft.Practices.ObjectBuilder2;
    using Sleemon.Common;
    using Sleemon.Data;
    using Sleemon.Portal.Common;
    using Sleemon.Portal.Core;

    public class ExamController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.UserUniqueId = User.GetUserUniqueId();
            
            return View("ExamList");
        }

        [HttpGet]
        public JsonContentAction GetExamList()
        {
            var examList = ServiceClient.Request<IExamService, IList<ExamListModel>>(service => service.GetExamList());

            return new JsonContentAction(examList);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View("ExamCreate");
        }

        [HttpGet]
        public JsonContentAction GetExamDetail(int examId)
        {
            var exam =
                ServiceClient.Request<IExamService, ExamDetailModel>(
                    service => service.GetExamDetailById(examId));

            return new JsonContentAction(exam);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ExamCreate(ExamDetailModel exam)
        {
            var msg = "Save successful.";
            var validation = ValidateModelForExam(exam);
            if (string.IsNullOrEmpty(validation))
            {
                EnrichExamDetailModel(exam);
                var result =
                    ServiceClient.Request<IExamService, ResultBase>(service => service.SaveExamDetail(exam));
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
        public ActionResult DeleteExam(int id)
        {
            var msg = "Delete successful.";
            var exam = ServiceClient.Request<IExamService, ExamDetailModel>(service => service.GetExamDetailById(id));
            if (exam != null && exam.LastUpdateUser == UserUniqueId)
            {
                var result = ServiceClient.Request<IExamService, ResultBase>(service => service.DeleteExamById(id));
                if (!result.IsSuccess)
                {
                    msg = result.Message;
                }
            }
            else
            {
                msg = @"Exam info is not found.";
            }

            return new JsonResult() {Data = msg};
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

        private string ValidateModelForExam(ExamDetailModel exam)
        {
            return string.Empty;
        }
        
        private void EnrichExamDetailModel(ExamDetailModel exam)
        {
            exam.LastUpdateUser = UserUniqueId;

            if (exam.Questions != null)
            {
                for (int i = 0; i < exam.Questions.Count; i++)
                {
                    var question = exam.Questions[i];
                    question.No = (short)(i + 1);

                    if (question.Choices != null)
                    {
                        for (int j = 0; j < question.Choices.Count; j++)
                        {
                            var choice = question.Choices[j];
                            choice.Choice = (byte)(j + 1);
                        }

                        var answers = question.Choices.Where(u => u.IsAnswer).ToList();
                        question.CorrectAnswer = answers.Select(u => u.Choice).JoinStrings(",");
                        question.Category = answers.Count == 1
                            ? (byte)ExamQuestionCategory.SingleOption
                            : (byte)ExamQuestionCategory.MultiOptions;
                    }
                }
            }
            
        }
    }
}