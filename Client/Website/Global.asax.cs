using System;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Electracraft.Client.Website;
using Electracraft.Client.Website.Controller_Utility;

namespace Website
{
    public class Global : System.Web.HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
           RouteTable.Routes.MapHttpRoute(
                name: "Default api",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new {id = System.Web.Http.RouteParameter.Optional}
                ).RouteHandler=new SessionStateRouteHandler();
           
        }

        protected void Application_PostAuthorizeRequest()
        {
            if (IsWebAPIRequest())
            {
                HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);    
            }
        }

        private bool IsWebAPIRequest()
        {
            return
                HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath != null && HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.StartsWith(
	                WebApiConfig.UrlPrefixRelative);
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

     
    }
}
