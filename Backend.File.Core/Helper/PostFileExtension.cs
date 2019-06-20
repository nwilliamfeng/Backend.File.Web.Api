using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Backend.File
{
    public static class PostFileExtension
    {
        public static string GetFileExtension(this HttpPostedFile file)
        {
            switch (file.ContentType)
            {
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                    return ".docx";
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                    return ".xlsx";
                case "application/msword":
                    return ".doc";
                case "application/zip":
                    return ".zip";
                case "application/vnd.ms-excel":
                    return ".xls";
                case "application/pdf":
                    return ".pdf";
                case "application/json":
                    return ".json";
                case "image/jpeg":
                    return ".jpg";
                case "image/bmp":
                    return ".bmp";
                case "image/png":
                    return ".png";
                case "text/plain":
                    return ".txt";
                default:
                    var strs = file.FileName.Split('.');
                    return strs.Length>1?strs[1]:null;

            }
        }
    }
}