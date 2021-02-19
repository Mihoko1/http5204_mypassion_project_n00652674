using System.Web;
using System.Web.Mvc;

namespace http5204_mypassion_project_n00652674
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
