using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Backend.File.Web.Controllers
{
    public class FileController : ApiController
    {

     

        [HttpPost]
        public async Task<IHttpActionResult> Upload()
        {
            var result =await FileUploadService.Upload(HttpContext.Current.Request);
            return this.Json(result);

        }
    }
}
