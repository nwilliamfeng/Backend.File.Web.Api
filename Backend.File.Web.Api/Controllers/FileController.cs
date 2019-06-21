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
using System.IO;

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
        public async Task<IHttpActionResult> UploadWithForm()
        {
            var result =await this._fileUploadService.UploadWithForm(HttpContext.Current.Request);
            return this.Json(result);

        }

        [HttpPost]
        public async Task<IHttpActionResult> Upload()
        {
            var streamProvider = new MultipartFormDataStreamProvider(ServerUploadFolder);
            await Request.Content.ReadAsMultipartAsync(streamProvider);

            return new FileResult
            {
                FileNames = streamProvider.FileData.Select(entry => entry.LocalFileName),
                Names = streamProvider.FileData.Select(entry => entry.Headers.ContentDisposition.FileName),
                ContentTypes = streamProvider.FileData.Select(entry => entry.Headers.ContentType.MediaType),
                Description = streamProvider.FormData["description"],
                CreatedTimestamp = DateTime.UtcNow,
                UpdatedTimestamp = DateTime.UtcNow,
                DownloadLink = "TODO, will implement when file is persisited"
            };
        }

        //public HttpResponseMessage DownloadFile()
        //{
        //    HttpResponseMessage result = null;
        //    var localFilePath = HttpContext.Current.Server.MapPath("~/timetable.jpg");

        //    if (!File.Exists(localFilePath))
        //    {
        //        result = Request.CreateResponse(HttpStatusCode.Gone);
        //    }
        //    else
        //    {
        //        result = Request.CreateResponse(HttpStatusCode.OK);
        //        result.Content = new StreamContent(new FileStream(localFilePath, FileMode.Open, FileAccess.Read));
        //        result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
        //        result.Content.Headers.ContentDisposition.FileName = "SampleImg";
        //        result.
        //    }

        //    return result;
        //}
    }
}
