using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DogShelterMvc.Attributes
{
    public class AuthorizeAttribute : ActionFilterAttribute
    {
        private readonly ulong _requiredPerms;
        private readonly bool _checkPerms;

        public AuthorizeAttribute(ulong requiredPerms = 0)
        {
            _requiredPerms = requiredPerms;
            _checkPerms = requiredPerms > 0;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.Session.GetString("UserId");
            
            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new RedirectToActionResult("Login", "Account", new { returnUrl = context.HttpContext.Request.Path });
                return;
            }

            if (_checkPerms)
            {
                var permsString = context.HttpContext.Session.GetString("Perms");
                if (string.IsNullOrEmpty(permsString) || !ulong.TryParse(permsString, out var perms) || perms < _requiredPerms)
                {
                    context.Result = new RedirectToActionResult("Login", "Account", null);
                    return;
                }
            }

            base.OnActionExecuting(context);
        }
    }
}

