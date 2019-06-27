using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Backend.File.Data.MySql
{
    public class FileInfoRepository : IFileInfoRepository
    {
    
        public bool IsCache => true;

        public async Task<bool> Add(FileInfo file)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<FileInfo> Load(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(FileInfo fileInfo)
        {
            throw new NotImplementedException();
        }
    }
}
