using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace File.Service.WebApi
{
    public class FileServerConfigurationSection:ConfigurationSection
    {
        public static FileServerConfigurationSection Instance
        {
            get { return ConfigurationManager.GetSection("fileServerConfig") as FileServerConfigurationSection; }
        }

        [ConfigurationProperty("writeDir", IsRequired = true)]
        public string WriteDir
        {
            get { return (string)base["writeDir"]; }
        }

        [ConfigurationProperty("name", IsRequired = true)]
        public string  Name
        {
            get { return (string)base["name"]; }
        }

        



    }
}