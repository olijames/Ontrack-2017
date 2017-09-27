
using System.Web.Http;
using System.Web.UI;


namespace Electracraft.Client.Website
{
    public static class WebApiConfig
    {
        public static string UrlPrefix { get { return "api"; } }
    public  static string UrlPrefixRelative { get { return "~/api"; } }

        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            // config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: WebApiConfig.UrlPrefix+"api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            // config.Filters.Add(new AuthorizeAttribute());
        }

    }
}
