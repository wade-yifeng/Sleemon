using Sleemon.Common;
using Sleemon.Core;
using Sleemon.Data;
using Sleemon.Portal.Common;
using Sleemon.Portal.Core;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Sleemon.Portal.Controllers
{
    public class QuestionnaireController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.UserUniqueId = User.GetUserUniqueId();
            return View("QuestionnaireList");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View("QuestionnaireCreate");
        }

        public JsonContentAction GetQuestionnaireList(int pageIndex, int pageSize, string questionnaireTitle)
        {
            var questionnaireList = ServiceClient.Request<IQuestionnaireService, IList<QuestionnaireListModel>>(
                        service => service.GetQuestionnaireList(pageIndex, pageSize, questionnaireTitle));

            return new JsonContentAction(questionnaireList);
        }

        public JsonContentAction GetQuestionnaire(int id)
        {
            var questionnaire = ServiceClient.Request<IQuestionnaireService, QuestionnaireDetailModel>(
                        service => service.GetQuestionnaireDetailById(id));
            return new JsonContentAction(questionnaire);
        }

        public JsonContentAction CreateQuestionnaire(QuestionnaireDetailModel questionnaire)
        {
            EnrichQuestionnaireDetailModel(questionnaire);
            var result = ServiceClient.Request<IQuestionnaireService, ResultBase>(
                        service => service.SaveQuestionnaireDetail(questionnaire));

            return new JsonContentAction(result);
        }

        public JsonContentAction DeleteQuestionnaire(int id)
        {
            var result = ServiceClient.Request<IQuestionnaireService, ResultBase>(
                        service => service.DeleteQuestionnaireById(id));

            return new JsonContentAction(result);
        }

        private void EnrichQuestionnaireDetailModel(QuestionnaireDetailModel questionnaire)
        {
            questionnaire.LastUpdateUser = UserUniqueId;
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
    }
}