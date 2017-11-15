using System;
using coffeepoint.app.Model;
using Microsoft.AspNetCore.Mvc.Filters;

namespace coffeepoint.app.Controllers
{
    public class SingleAccessFilter: IActionFilter
    {
        private readonly SingleAccessService service;

        public SingleAccessFilter(SingleAccessService service)
        {
            this.service = service;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Controller != null)
            {
                var isProvideSingleAccess = context.Controller.GetType().GetCustomAttributes(typeof(SingleAccessAttribute), true).Length > 0;
                if (isProvideSingleAccess)
                {
                    context.HttpContext.Items["access_token"] = service.StartSession();
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.Items.ContainsKey("access_token"))
            {
                var token = (IDisposable)context.HttpContext.Items["access_token"];
                token.Dispose();
            }
        }
    }
}