using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microcomm.Model;
using Microcomm;
using Newtonsoft.Json;
using StackExchange.Redis;
using Microcomm.Redis;

namespace Backend.File.Data.Redis
{
    public class FileInfoCache : RedisCache, IFileInfoRepository
    {
        private const string KEY = "hash_fileInfos";

        private string GetSortedSetKey(string category) => "sortedSet_" + category;

        public async Task<bool> Add(FileInfo file)
        {
            var result = await this.Database.HashSetAsync(KEY, file.Id, JsonConvert.SerializeObject(file));
            if (!result)
                return false;
            result = await this.Database.SortedSetAddAsync(this.GetSortedSetKey(file.Category), file.Id, file.CreateTime.ToTimeStamp());
            return result;
        }

        public async Task<bool> Delete(string id)
        {
            var file = await this.Load(id);
            if (file == null)
                return false;
            bool result = await this.Database.HashDeleteAsync(KEY, id);
            if (!result)
                return false;
            result = await this.Database.SortedSetRemoveAsync(this.GetSortedSetKey(file.Category), id);
            return result;
        }

        public async Task<FileInfo> Load(string id)
        {
            var value = await this.Database.HashGetAsync(KEY, id);
            return JsonConvert.DeserializeObject<FileInfo>(value);
        }

        public async Task<QueryResult<FileInfo>> Search(FileInfoQueryCondition queryCondition)
        {
           // 过期逻辑放到这
            if (string.IsNullOrEmpty(queryCondition.Category))
                return new QueryResult<FileInfo>();
            var sortKey = this.GetSortedSetKey(queryCondition.Category);
            Order order = queryCondition.Sort == SortMode.Descending ? StackExchange.Redis.Order.Descending : StackExchange.Redis.Order.Ascending;
            if (!queryCondition.IsTimeAvailable)
            {
                var total = await this.Database.SortedSetLengthAsync(sortKey);
                var ids = await this.Database.SortedSetRangeByPagingAsync(sortKey, queryCondition.PageIndex, queryCondition.PageSize, order);

                return new QueryResult<FileInfo>
                {
                    TotalCount = (int)total,
                    Items = await this.LoadFileInfos(ids)
                };
            }
            else
            {
                var start = queryCondition.StartTime.ToTimeStamp();
                var end = queryCondition.EndTime.ToTimeStamp();
                var total = await this.Database.SortedSetLengthAsync(sortKey, start, end);

           
                var ids = this.Database.SortedSetRangeByScorePaging(sortKey,
                   start,
                   end,
                   queryCondition.PageIndex,
                   queryCondition.PageSize,
                   Exclude.None,
                   order
                );

                return new QueryResult<FileInfo>
                {
                    TotalCount = (int)total,
                    Items = await this.LoadFileInfos(ids)
                };
            }

        }

        private async Task<List<FileInfo>> LoadFileInfos(RedisValue[] ids)
        {
            List<FileInfo> lst = new List<FileInfo>();
            foreach (string id in ids)
                lst.Add(await this.Load(id));
            return lst;
        }

        public Task<bool> Update(FileInfo fileInfo)
        {
            throw new NotImplementedException();
        }
    }
}
