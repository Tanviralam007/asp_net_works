using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeroHunger_Food_Distribution.Helpers
{
    public class SessionHelper
    {
        public static int? GetUserId()
        {
            if (HttpContext.Current.Session["UserId"] != null)
            {
                return (int)HttpContext.Current.Session["UserId"];
            }
            return null;
        }

        public static string GetUserRole()
        {
            if (HttpContext.Current.Session["UserRole"] != null)
            {
                return HttpContext.Current.Session["UserRole"]?.ToString();
            }
            return null;
        }

        public static string GetUserName()
        {
            if (HttpContext.Current.Session["UserName"] != null)
            {
                return HttpContext.Current.Session["UserName"]?.ToString();
            }
            return null;
        }

        public static void SetUserSession(int userId, string userRole, string userName)
        {
            HttpContext.Current.Session["UserId"] = userId;
            HttpContext.Current.Session["UserRole"] = userRole;
            HttpContext.Current.Session["UserName"] = userName;
        }

        public static void ClearUserSession()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }

        public static bool IsLoggedIn()
        {
            return GetUserId().HasValue;
        }
    }
}