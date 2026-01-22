using System.Web;
using System.Web.Mvc;

namespace ZeroHunger_Food_Distribution
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
