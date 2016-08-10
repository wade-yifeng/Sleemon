using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sleemon.Core;
using Sleemon.Data;
using Sleemon.Portal.Common;
using Sleemon.Portal.Core;

namespace Sleemon.Portal.Controllers
{
    public class KnowledgeController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View("KnowledgeList");
        }

        [HttpGet]
        public JsonContentAction GetKnowledgeList()
        {
            var knowledges =
                ServiceClient.Request<IKnowledgeService, IList<KnowledgeBaseDetailModel>>(
                    service => service.GetKnowledgeBaseList());

            return new JsonContentAction(knowledges);
        }

        [HttpGet]
        public JsonContentAction GetKeyWordsList()
        {
            var keywords =
                ServiceClient.Request<IKnowledgeService, IList<KeyWord>>(
                    service => service.GetKeyWordsList());

            return new JsonContentAction(keywords);
        }

        [HttpGet]
        public ActionResult Detail()
        {
            return View("KnowledgeDetail");
        }

        [HttpGet]
        public JsonContentAction GetKnowledgeDetailById(int knowledgeBaseId)
        {
            var knowledge = ServiceClient.Request<IKnowledgeService, KnowledgeBaseDetailModel>(
                    service => service.GetKnowledgeBaseDetail(knowledgeBaseId));

            return new JsonContentAction(knowledge);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View("KnowledgeCreate");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult KnowledgeCreate(KnowledgeBaseDetailModel knowledge)
        {
            var msg = "Save successful.";
            var validation = ValidateModelForKnowledge(knowledge);
            if (string.IsNullOrEmpty(validation))
            {
                EnrichKnowledgeDetailModel(knowledge);
                var result =
                    ServiceClient.Request<IKnowledgeService, ResultBase>(
                        service => service.SaveKnowledgeBaseDetail(knowledge));
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

        private void EnrichKnowledgeDetailModel(KnowledgeBaseDetailModel knowledge)
        {
            knowledge.LastUpdateUser = User.GetUserUniqueId();
        }

        private string ValidateModelForKnowledge(KnowledgeBaseDetailModel knowledge)
        {
            return string.Empty;
        }
    }
}