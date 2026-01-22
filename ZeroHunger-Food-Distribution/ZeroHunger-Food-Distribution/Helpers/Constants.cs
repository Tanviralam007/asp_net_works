using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeroHunger_Food_Distribution.Helpers
{
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string Employee = "Employee";
        public const string Restaurant = "Restaurant";
    }

    public static class RequestStatus
    {
        public const string Pending = "Pending";
        public const string Assigned = "Assigned";
        public const string Collected = "Collected";
        public const string Distributed = "Distributed";
        public const string Completed = "Completed";
        public const string Cancelled = "Cancelled";
    }
}