using System.Web;
using System.Web.Mvc;

namespace ZeroHunger_Food_Distribution.Helpers
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] _roles;

        public CustomAuthorizeAttribute(params string[] roles)
        {
            _roles = roles;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // check if user is authenticated
            if (httpContext.Session["UserId"] == null)
            {
                return false;
            }

            // if no specific roles required, just authentication is enough
            if (_roles == null || _roles.Length == 0)
            {
                return true;
            }

            // check if user has required role
            var userRole = httpContext.Session["UserRole"] as string;

            foreach (var role in _roles)
            {
                if (userRole == role)
                {
                    return true;
                }
            }

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["UserId"] == null)
            {
                // not logged in - redirect to login
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(
                        new { controller = "Account", action = "Login" }
                    )
                );
            }
            else
            {
                // logged in but wrong role - show error and redirect
                filterContext.Controller.TempData["ErrorMessage"] = "Access denied. You don't have permission to access this page.";
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(
                        new { controller = "Account", action = "Login" }
                    )
                );
            }
        }
    }
}
