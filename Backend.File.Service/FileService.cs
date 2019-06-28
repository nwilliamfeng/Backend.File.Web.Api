using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Backend.File.Data;
using Microcomm;
using Microcomm.Model;


namespace Backend.File.Service
{
    public class FileService
    {

        private IEnumerable<IFileInfoRepository> _fileInfoRepositories;

        public FileService(IEnumerable<IFileInfoRepository> fileInfoRepositories)
        {
            this._fileInfoRepositories = fileInfoRepositories;
        }

        private IFileInfoRepository Cache => this._fileInfoRepositories.FirstOrDefault(x => x.IsCache);

        private IFileInfoRepository Db => this._fileInfoRepositories.FirstOrDefault(x => !x.IsCache);

        public async Task<QueryResult<FileInfo>> Search(FileInfoQueryCondition queryCondition)
        {
            var result= await this.Cache.Search(queryCondition);
            if (result != null)
            {
                if(result.Items!=null && result.Items.Any(x=> x.Expire > 0 && this.IsExpire(x))) //处理过期的文件
                {
                    result.Items = result.Items.Where(x => x.Expire == 0 || !this.IsExpire(x)).ToList();
                };
            }
            return result;
        }

        private bool IsExpire(FileInfo file)
        {
            return DateTime.Now.ToTimeStamp() - file.CreateTime.ToTimeStamp() > file.Expire;
        }


        private void RemoveFileFromDisk(FileInfo file)
        {

        }



        public async Task<JsonResultData<IEnumerable<FileInfo>>> UploadWithForm(HttpRequest httpRequest)
        {
            if (httpRequest.Headers.GetValues("category") == null)
                return new JsonResultData<IEnumerable<FileInfo>>().SetFail("未设置子路径");
            var category = httpRequest.Headers.GetValues("category").FirstOrDefault();
            if (string.IsNullOrEmpty(category))
                return new JsonResultData<IEnumerable<FileInfo>>().SetFail("未设置子路径");
            if (httpRequest.Files.Count == 0)
                return new JsonResultData<IEnumerable<FileInfo>>().SetFail("找不到文件");
            List<FileInfo> lst = new List<FileInfo>();
            foreach (string file in httpRequest.Files)
            {
                var postFile = httpRequest.Files[file];

                var id = Guid.NewGuid().ToString("N");
                var dir = $"{FileServerConfigurationSection.Instance.WriteDir}\\{category}";
                var extension = postFile.GetFileExtension();
                var path = $"{dir}\\{id}{extension}";
                if (!System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);
                postFile.SaveAs(path);
                FileInfo fi = new FileInfo
                {
                    Id = id,
                    Category= category,
                    Size = postFile.ContentLength,
                    CreateTime = DateTime.Now,
                    Extension = extension,
                    Name = postFile.FileName,
                    Url = $"{FileServerConfigurationSection.Instance.Name}\\{category}\\{id}{extension}"
                };
                lst.Add(fi);
                await this.Cache.Add(fi);
            }
            return new JsonResultData<IEnumerable<FileInfo>> { Data = lst }.SetSuccess();
        }

        public async Task<JsonResultData<FileInfo>> UploadWithBase64Content(Base64File file)
        {

            if (string.IsNullOrEmpty(file.Category))
                return new JsonResultData<FileInfo>().SetFail("未设置子路径");

            if (string.IsNullOrEmpty(file.FileName))
                return new JsonResultData<FileInfo>().SetFail("未设置文件名");

            var id = Guid.NewGuid().ToString("N");
            var category = $"{FileServerConfigurationSection.Instance.WriteDir}\\{file.Category}";
            var extension = file.FileName.Contains('.') ? "." + file.FileName.Split('.').Last() : null;
            var path = $"{category}\\{id}{extension}";
            if (!System.IO.Directory.Exists(category))
                System.IO.Directory.CreateDirectory(category);
            var bytes = Convert.FromBase64String(file.Base64Content);
            System.IO.File.WriteAllBytes(path, bytes);
            FileInfo fi = new FileInfo
            {
                Id = id,
                Size = bytes.Length,
                Category = file.Category,
                CreateTime = DateTime.Now,
                Extension = extension,
                Name = file.FileName,
                Url = $"{FileServerConfigurationSection.Instance.Name}\\{file.Category}\\{id}{extension}"
            };
            await this.Cache.Add(fi);
            return new JsonResultData<FileInfo> { Data = fi }.SetSuccess();

        }




    }
}