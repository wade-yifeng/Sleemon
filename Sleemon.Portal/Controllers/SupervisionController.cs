using Sleemon.Common;
using Sleemon.Core;
using Sleemon.Data;
using Sleemon.Portal.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sleemon.Portal.Controllers
{
    public class SupervisionController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View("CommentList");
        }

        [HttpGet]
        public JsonContentAction GetIlegalCommentList()
        {
            var comments =
                ServiceClient.Request<ICommentService, IList<UserComment>>(
                    service => service.GetUserCommentsByStatus(LegalStatus.Illegal));

            return new JsonContentAction(comments);
        }

        [HttpPost]
        public ActionResult UpdateCommentLegalStatus(int commentId, byte legalStatus)
        {
            var msg = "Save successful.";

            var result =
               ServiceClient.Request<ICommentService, ResultBase>(
                   service => service.UpdateCommentLegalStatus(commentId, legalStatus));

            if (!result.IsSuccess)
            {
                msg = result.Message;
            }

            return new JsonResult() { Data = msg };
        }
    }
}