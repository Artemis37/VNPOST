using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VNPOSTWebUI.Models
{
    public class CustomAuthorizeHandler : AuthorizationHandler<OperationAuthorizationRequirement, News>
    {
        UserManager<IdentityUser> _userManager;

        public CustomAuthorizeHandler(UserManager<IdentityUser>
            userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, News resource)
        {
            //context.Succeed(requirement);
            //return Task.CompletedTask;
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            if (resource.CreatedBy == _userManager.GetUserId(context.User))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
