using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BootstrapIntro.Filters
{
    public class ValidationActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            //base.OnActionExecuting(actionContext);

            var modelState = actionContext.ModelState;
            if (!modelState.IsValid)
            {
                actionContext.Response = actionContext.Request.CreateResponse(
                    HttpStatusCode.BadRequest, modelState);
            }

        }
    }
}