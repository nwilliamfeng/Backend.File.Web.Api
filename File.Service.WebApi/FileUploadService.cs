using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;


namespace File.Service.WebApi
{
    public static class FileUploadService
    {
        public static   Task<JsonResultData<IEnumerable<FileInfo>>> Upload(HttpRequest httpRequest)
        {
            return Task.Run(() =>
            {
                var dir = httpRequest.Headers.GetValues("dir").FirstOrDefault();
                if (string.IsNullOrEmpty(dir))
                    return new JsonResultData<IEnumerable<FileInfo>>().SetFail("未设置子路径");
                if (httpRequest.Files.Count == 0)
                    return new JsonResultData<IEnumerable<FileInfo>>().SetFail("找不到文件");
                List<FileInfo> lst = new List<FileInfo>();
                foreach (string file in httpRequest.Files)
                {
                    var postFile = httpRequest.Files[file];
                    
                    var id = Guid.NewGuid().ToString("N");
                    var path = FileServerConfigurationSection.Instance.WriteDir + dir + "\\" + id;
                //    if (!System.IO.Directory.Exists(path))
                  //      System.IO.Directory.CreateDirectory();
                    postFile.SaveAs(path);
                    FileInfo fi = new FileInfo { CreateTime = DateTime.Now, Name = postFile.FileName, Url = $"{FileServerConfigurationSection.Instance.Name}\\{dir}\\{id}" };
                    lst.Add(fi);
                }
                return new JsonResultData<IEnumerable<FileInfo>> { Data = lst }.SetSuccess();
            });
           
        }

        private static string GetFileExtension()
        {
            return null;
        }

    }
}