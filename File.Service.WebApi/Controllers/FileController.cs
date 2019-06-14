using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace File.Service.WebApi.Controllers
{
    public class FileController : ApiController
    {

     

        [HttpPost]
        public async Task<IHttpActionResult> Upload()
        {
            var httpRequest = HttpContext.Current.Request;
            var dir = httpRequest.Headers.GetValues("dir").FirstOrDefault();
            if (httpRequest.Files.Count == 0)
                return this.JsonResult(false.ToJson("未找到文件"));

            foreach (string file in httpRequest.Files)
            {
                var postFile = httpRequest.Files[file];
                postFile.
            //    var serverPath = HttpContext.Current.Server.MapPath("~/" + postFile.FileName);
                postFile.SaveAs(serverPath);
            }

        }
    }
}
