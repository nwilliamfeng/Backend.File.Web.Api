using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microcomm;

namespace Backend.File.Test
{
    class Program
    {
        private static string uploadurl = System.Configuration.ConfigurationManager.AppSettings["uploadUrl"];
        private static string formPath = "/api/file/UploadWithForm";
        private static string uppath = "/api/file/Upload";
        private static string token = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJleHAiOjE1NjIwMzE2MzksInVzZXJJZCI6ImZ3IiwidGltZXN0YW1wIjoxNTYxNDI2ODM5fQ.MmYK1RTIwXOP4HPiAY9NsYfWvkRQ9UwVPtr_KqsZ2CA";

        static void Main(string[] args)
        {
            while (true)
            {

                Console.WriteLine("1. PostWithForm");
                Console.WriteLine("2. PostWithBase64");
                var v = 0;
                int.TryParse(Console.ReadLine(),out v);
                if (v == 1)
                    UploadTestWithForm();
                else if (v == 2)
                    UploadTestWithBase64();
            }


        }

        private static void UploadTestWithBase64()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("please input file path and press enter");
                    var path = Console.ReadLine();
             
                    if ( !System.IO.File.Exists(path))
                    {
                        Console.WriteLine("不存在的文件");
                        break;
                    }
                  
                    Base64File file = Base64File.FromFile(path,"dir2");
                   
                    var result = new HttpClientUtil(uploadurl).PostWithJson<JsonResultData<FileInfo>>(uppath, file, new Dictionary<string, string> { ["Authorization"] = token  });
                    Console.WriteLine("the result is :");
                    Console.WriteLine(result.Result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

            }
        }

        private static void UploadTestWithForm()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("please input  files path ,each file use sparator char to join  and press enter to run...");
                    var pathStr = Console.ReadLine();
                    var paths = pathStr.Split(',');
                    if (paths.Any(x => !System.IO.File.Exists(x)))
                    {
                        Console.WriteLine("不存在的文件");
                        break;
                    }
                    var result = new HttpClientUtil(uploadurl).Upload(formPath, paths, new Dictionary<string, string> { ["Authorization"] = token, ["category"] = "dir2" });
                    Console.WriteLine("the result is :");
                    Console.WriteLine(result.Result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

            }
        }
    }


}
