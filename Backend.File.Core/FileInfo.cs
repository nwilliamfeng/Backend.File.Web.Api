using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microcomm;

namespace Backend.File
{
    public class FileInfo
    {
        public string Id { get; set; }

        public string Category { get; set; }


        public int Size { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }

        public string Url { get; set; }

        public DateTime CreateTime { get; set; }


        public int Expire { get; set; }

       // public bool IsExire => DateTimeUtils.ToUnixTime(DateTime.Now) - DateTimeUtils.ToUnixTime(this.CreateTime) > Expire;

       

    }
}
