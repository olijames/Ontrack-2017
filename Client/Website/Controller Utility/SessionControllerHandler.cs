using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.WebHost;
using System.Web.Routing;
using System.Web.SessionState;

namespace Electracraft.Client.Website.Controller_Utility
{
    public class SessionControllerHandler:HttpControllerHandler,IRequiresSessionState
    {
        public SessionControllerHandler(RouteData routeData) : base(routeData)
        {
        }

        public SessionControllerHandler(RouteData routeData, HttpMessageHandler handler) : base(routeData, handler)
        {
        }
    }
}