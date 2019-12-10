using ApplicationChallenge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;

namespace ApplicationChallenge.Attributes
{
    public class PermissionAttribute: ActionFilterAttribute
    {
        private string _permission;
        public PermissionAttribute(string permission) {
            
            this._permission = permission;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var svc = context.HttpContext.RequestServices;
            var dbcontext = svc.GetService(typeof(ApplicationContext)) as ApplicationContext;

            var identity = context.HttpContext.User.Identity as ClaimsIdentity;
            
            var userRole = identity.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Value;
            var Rolepermissions = dbcontext.UserTypes.Where(x => x.Soort == userRole).Include(x => x.Permissions).SingleOrDefault();

            if (Rolepermissions == null)
            {
                throw new Exception("role permissions not defined in db");
            }

            if (!Rolepermissions.Permissions.Any(x => x.Title == _permission))
            {
                context.Result = new ForbidResult();


            } else
            {
               base.OnActionExecuting(context);
            }

        }
    }
}