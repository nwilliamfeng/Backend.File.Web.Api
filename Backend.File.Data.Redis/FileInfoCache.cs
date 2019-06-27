using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Backend.File.Data.Redis
{
    public class FileInfoCache : RedisCache, IFileInfoRepository
    {
        private const string KEY = "hash_fileInfos";

        public async Task<bool> Add(FileInfo file)
        {
           return await this.Database.HashSetAsync(KEY, file.Id, JsonConvert.SerializeObject(file));
        }

        public async Task<bool> Delete(string id)
        {
            return await this.Database.HashDeleteAsync(KEY, id);
        }

        public async Task<FileInfo> Load(string id)
        {
            var value = await this.Database.HashGetAsync(KEY, id);
            return JsonConvert.DeserializeObject<FileInfo>(id);
        }

        public Task<bool> Update(FileInfo fileInfo)
        {
            throw new NotImplementedException();
        }
    }
}
