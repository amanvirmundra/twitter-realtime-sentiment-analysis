using System.Web;
using System.Web.Mvc;

namespace Walnut.Sentiments
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
