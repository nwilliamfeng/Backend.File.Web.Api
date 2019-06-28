using Microcomm.Web.Http.Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;


using System.Web.Routing;

namespace Backend.File.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Backend.File.Data.Redis.RedisCache.ConfigName = "RedisServer";
            
            AutofacWebapiConfig.Initialize(this, GlobalConfiguration.Configuration, "Backend.File", new string[] { "Repository", "Cache", "Service" });
            GlobalConfiguration.Configure(WebApiConfig.Register);
        
            
            
        }
    }
}
