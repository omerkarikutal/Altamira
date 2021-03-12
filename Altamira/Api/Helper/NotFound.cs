using Core.Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Helper
{
    public class NotFoundAttribute : TypeFilterAttribute
    {
        public NotFoundAttribute() : base(typeof(NotFoundAttributeFilter))
        {
        }
        private class NotFoundAttributeFilter : IAsyncActionFilter
        {
            private readonly IUserService userService;
            public NotFoundAttributeFilter(IUserService userService)
            {
                this.userService = userService;
            }
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (context.ActionArguments.ContainsKey("id"))
                {
                    string id = context.ActionArguments["id"] as string;
                    var result = await userService.GetUserById(id);
                    if (result == null)
                    {
                        context.Result = new NotFoundObjectResult(new { ErrorMessage = "Record not found" });
                        return;
                    }
                }
                await next();
            }
        }
    }

}
