using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microcomm.Web.Http.Filters;
using WebApi.OutputCache.V2;
using Backend.File.Service;

namespace Backend.File.Web.Controllers
{
    [Authentication]
    public class FileController : ApiController
    {
        private FileUploadService _fileUploadService;

        public FileController(FileUploadService fileUploadService)
        {
            this._fileUploadService = fileUploadService;
        }

     
        [HttpPost]
        public async Task<IHttpActionResult> Upload()
        {
            var result =await this._fileUploadService.Upload(HttpContext.Current.Request);
            return this.Json(result);

        }
    }
}
