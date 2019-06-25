using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using Microcomm;


namespace Backend.File.Service
{
    public class FileUploadService
    {
        public Task<JsonResultData<IEnumerable<FileInfo>>> UploadWithForm(HttpRequest httpRequest)
        {
            return Task.Run(() =>
            {
                if (httpRequest.Headers.GetValues("dir") == null)
                    return new JsonResultData<IEnumerable<FileInfo>>().SetFail("未设置子路径");
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
                    FileInfo fi = new FileInfo
                    {
                        Id = id,
                        Size = postFile.ContentLength,
                        CreateTime = DateTime.Now,
                        Extension = extension,
                        Name = postFile.FileName,
                        Url = $"{FileServerConfigurationSection.Instance.Name}\\{subDir}\\{id}{extension}"
                    };
                    lst.Add(fi);
                }
                return new JsonResultData<IEnumerable<FileInfo>> { Data = lst }.SetSuccess();
            });

        }

        public Task<JsonResultData<FileInfo>> UploadWithBase64Content(Base64File file)
        {
            return Task.Run(() =>
            {
                if (string.IsNullOrEmpty(file.Dir))
                    return new JsonResultData<FileInfo>().SetFail("未设置子路径");

                if (string.IsNullOrEmpty(file.FileName))
                    return new JsonResultData<FileInfo>().SetFail("未设置文件名");

                var id = Guid.NewGuid().ToString("N");
                var dir = $"{FileServerConfigurationSection.Instance.WriteDir}\\{file.Dir}";
                var extension = file.FileName.Contains('.') ? "." + file.FileName.Split('.').Last() : null;
                var path = $"{dir}\\{id}{extension}";
                if (!System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);
                var bytes = Convert.FromBase64String(file.Base64Content);
                System.IO.File.WriteAllBytes(path, bytes);
                FileInfo fi = new FileInfo
                {
                    Id = id,
                    Size = bytes.Length,
                    CreateTime = DateTime.Now,
                    Extension = extension,
                    Name = file.FileName,
                    Url = $"{FileServerConfigurationSection.Instance.Name}\\{file.Dir}\\{id}{extension}"
                };

                return new JsonResultData<FileInfo> { Data = fi }.SetSuccess();
            });

        }




    }
}