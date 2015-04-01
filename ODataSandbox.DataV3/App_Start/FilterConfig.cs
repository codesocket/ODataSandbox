using System.Web;
using System.Web.Mvc;

namespace ODataSandbox.DataV3
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
