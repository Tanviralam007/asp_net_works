using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger_Food_Distribution.Helpers;

namespace ZeroHunger_Food_Distribution.Controllers
{
    public class BaseController : Controller
    {
        // check if user is logged in
        protected bool IsAuthenticated()
        {
            return SessionHelper.IsLoggedIn();
        }

        // Get current user id
        protected int GetCurrentUserId()
        {
            var userId = SessionHelper.GetUserId();
            if (!userId.HasValue)
            {
                throw new UnauthorizedAccessException("User not logged in.");
            }
            return userId.Value;
        }

        // get current user role
        protected string GetCurrentUserRole()
        {
            var role = SessionHelper.GetUserRole();
            if (string.IsNullOrEmpty(role))
            {
                throw new UnauthorizedAccessException("User role not found.");
            }
            return role;
        }

        // get current user name
        protected string GetCurrentUserName()
        {
            var userName = SessionHelper.GetUserName();
            if (string.IsNullOrEmpty(userName))
            {
                throw new UnauthorizedAccessException("User name not found.");
            }
            return userName;
        }

        // check if user has specific role
        protected bool IsInRole(string role)
        {
            var currentRole = GetCurrentUserRole();
            return string.Equals(currentRole, role, StringComparison.OrdinalIgnoreCase);
        }

        // redirect to login if not authenticated
        protected ActionResult RedirectToLoginIfNotAuthenticated()
        {
            if (!IsAuthenticated())
            {
                return RedirectToAction("Login", "Account");
            }
            return null;
        }

        // redirect if not in specific role
        protected ActionResult RedirectIfNotInRole(string role)
        {
            if (!IsInRole(role))
            {
                TempData["ErrorMessage"] = "You do not have permission to access this resource.";
                return RedirectToAction("Login", "Account");
            }
            return null;
        }
    }
}