using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.Web.Http;
using Microcomm.Web.Http.Filters;

namespace Backend.File.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            ProgressMessageHandler progress = new ProgressMessageHandler();
            progress.HttpSendProgress += new EventHandler<HttpProgressEventArgs>(HttpSendProgress);

            // Web API 路由
            config.MapHttpAttributeRoutes();
            config.MessageHandlers.Add(progress);
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Filters.Add(new GlobalExceptionFilter());
            config.Filters.Add(new GlobalLogFilter());
        }

        private static void HttpSendProgress(object sender, HttpProgressEventArgs e)
        {
            HttpRequestMessage request = sender as HttpRequestMessage;
            // Do something with the event
            // e.ProgressPercentage, e.TotalBytes, e.BytesTransferred
        }
    }
}
