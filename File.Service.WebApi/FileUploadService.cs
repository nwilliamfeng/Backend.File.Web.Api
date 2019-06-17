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
                var subDir = httpRequest.Headers.GetValues("dir").FirstOrDefault();
                if (string.IsNullOrEmpty(subDir))
                    return new JsonResultData<IEnumerable<FileInfo>>().SetFail("未设置子路径");
                if (httpRequest.Files.Count == 0)
                    return new JsonResultData<IEnumerable<FileInfo>>().SetFail("找不到文件");
                List<FileInfo> lst = new List<FileInfo>();
                foreach (string file in httpRequest.Files)
                {
                    var postFile = httpRequest.Files[file];
                    
                    var id = Guid.NewGuid().ToString("N");
                    var dir = $"{FileServerConfigurationSection.Instance.WriteDir}\\{subDir}";
                    var extension = postFile.GetFileExtension();
                    var path = $"{dir}\\{id}{extension}";
                    if (!System.IO.Directory.Exists(dir))
                        System.IO.Directory.CreateDirectory(dir);
                    postFile.SaveAs(path);
                    FileInfo fi = new FileInfo {
                        Id = id,
                        Size=postFile.ContentLength,
                        CreateTime = DateTime.Now,
                        Extension =extension,
                        Name = postFile.FileName,
                        Url = $"{FileServerConfigurationSection.Instance.Name}\\{subDir}\\{id}{extension}"
                    };
                    lst.Add(fi);
                }
                return new JsonResultData<IEnumerable<FileInfo>> { Data = lst }.SetSuccess();
            });
           
        }

      
    }
}