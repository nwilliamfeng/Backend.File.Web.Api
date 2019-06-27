using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.File.Data
{
    public interface IFileInfoRepository
    {
        bool IsCache { get; }

        Task<bool> Add(FileInfo file);

        Task<bool> Update(FileInfo fileInfo);

        Task<bool> Delete(string id);

        Task<FileInfo> Load(string id);

        Task<>
    }
}
