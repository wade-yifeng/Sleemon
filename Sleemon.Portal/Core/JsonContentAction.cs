using Sleemon.Portal.Common;
using System.Web.Mvc;

namespace Sleemon.Portal.Core
{
    public class JsonContentAction : ActionResult
    {
        public JsonContentAction(object result)
            : this(result.ToJsonContent())
        {
        }

        public JsonContentAction(string json)
        {
            this.JsonContent = json;
        }

        private string JsonContent { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.Write(this.JsonContent);
        }
    }
}