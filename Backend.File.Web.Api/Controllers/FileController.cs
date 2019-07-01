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
using Microcomm;
using Microcomm.Web.Http;

namespace Backend.File.Web.Controllers
{
    [Authentication]
    public class FileController : ApiController
    {
        private FileService _fileUploadService;

        public FileController(FileService fileUploadService)
        {
            this._fileUploadService = fileUploadService;
        }

     
        [HttpPost]
        public async Task<IHttpActionResult> UploadWithForm()
        {
            var result =await this._fileUploadService.UploadWithForm(HttpContext.Current.Request);
            return this.JsonResult(result.ToJsonResultData());
        }


        [HttpPost]
        public async Task<IHttpActionResult> Upload([FromBody]Base64File file)
        {
            var result = await this._fileUploadService.UploadWithBase64Content(file);
            return this.JsonResult(result.ToJsonResultData());

        }

        [HttpPost]
        public async Task<IHttpActionResult> Search([FromBody]FileInfoQueryCondition queryCondition)
        {
            var result = await this._fileUploadService.Search(queryCondition);
            return this.JsonResult(result.ToJsonResultData());

        }

    }
}
