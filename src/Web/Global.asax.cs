using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Upload", "Upload", new {controller = "Root", action = "Upload"});
            routes.MapRoute("ImageStatus", "ImageStatus/{fileId}", new {controller = "Root", action = "ImageStatus"});
            routes.MapRoute("Default", "{controller}/{action}/{id}", 
                new { controller = "Root", action = "Index", id = UrlParameter.Optional }
            );

        }

        protected void Application_Start()
        {
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}