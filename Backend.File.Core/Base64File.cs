﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Backend.File
{
    public class Base64File
    {
        public Base64File()
        {

        }

        public static Base64File FromFile(string filePath,string category)
        {
            if (!System.IO.File.Exists(filePath))
                return null;
            return new Base64File {FileName= System.IO.Path.GetFileName(filePath)
                , Base64Content = Convert.ToBase64String(System.IO.File.ReadAllBytes(filePath))
            ,Category=category};
        }


        public string Category { get; set; }

        public string FileName { get; set; }

        public string Base64Content { get; set; }

        public long Expire { get; set; } = 0;
    }
}
