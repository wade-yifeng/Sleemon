using System.Collections.Generic;
using System.Web.Mvc;
using Sleemon.Common;
using Sleemon.Core;
using Sleemon.Data;
using Sleemon.Portal.Common;
using Sleemon.Portal.Core;

namespace Sleemon.Portal.Controllers
{
    public class QnAController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View("QnAList");
        }

        [HttpGet]
        public JsonContentAction GetQnAList()
        {
            var userQuestions =
                ServiceClient.Request<IUserQuestionService, IList<UserQuestionDetailModel>>(
                    service => service.GetAllUserQuestionsList());

            return new JsonContentAction(userQuestions);
        }

        [HttpGet]
        public ActionResult Detail()
        {
            return View("QnADetail");
        }

        [HttpGet]
        public JsonContentAction GetUserQuestionById(int questionId)
        {
            var question = ServiceClient.Request<IUserQuestionService, UserQuestionDetailModel>(
                    service => service.GetUserQuestionById(questionId));

            return new JsonContentAction(question);
        }

        [HttpGet]
        public ActionResult Reply()
        {
            return View("QnAReply");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SaveReplyAnswer(UserQuestionDetailModel answer)
        {
            var question = ServiceClient.Request<IUserQuestionService, IList<UserQuestionDetailModel>>(
                    service => service.GetAllUserQuestionsList());

            var msg = "Save successful.";
            var validation = ValidateModelForAnswer(answer);
            if (string.IsNullOrEmpty(validation))
            {
                EnrichAnswerDetailModel(answer);
                var result =
                    ServiceClient.Request<IUserQuestionService, ResultBase>(
                    service => service.SaveReplyAnswer(answer));
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

        private void EnrichAnswerDetailModel(UserQuestionDetailModel answer)
        {
            answer.Replier = User.GetUserUniqueId();
            answer.Status = (byte)UserQuestionStatus.Resolved;
        }

        private string ValidateModelForAnswer(UserQuestionDetailModel answer)
        {
            return string.Empty;
        }
    }
}