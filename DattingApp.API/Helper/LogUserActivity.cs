using DattingApp.DataLayer;
using DattingApp.Dto;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DattingApp.API.Helper
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            DattingDAL objDattingDAL = new DattingDAL();            
            var resultContext = await next();
            var userId = int.Parse(context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);            
            var user = objDattingDAL.UpdateUserLogActivity(userId);            
        }
    }
}
