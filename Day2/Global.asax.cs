using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.Web.Mvc;

namespace Day2
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
        protected void Application_BeginRequest()
        {
            //make sure its a preflight request  remove it
            if (Request.Headers.AllKeys.Contains("Origin") && Request.HttpMethod=="OPTIONS")
            {
                Response.Flush();
            }
        }
    }
}
