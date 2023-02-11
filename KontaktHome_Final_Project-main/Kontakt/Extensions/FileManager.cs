using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.Extensions
{
    public static class FileManager
    {
        public static bool CheckFileSize(this IFormFile file, double size)
        {
            return Math.Round((double)file.Length / 1024) < size;
        }

        public static bool CheckFileContentType(this IFormFile file, string contentType)
        {
            return file.ContentType == contentType;
        }

        public static string CreateFile(this IFormFile file, IWebHostEnvironment env, params string[] folders)
        {
            string fileName = $"{Guid.NewGuid()}_{DateTime.Now.ToString("yyyyMMddHHmmssffff")}_{file.FileName}";

            string path = env.WebRootPath;

            foreach (string folder in folders)
            {
                path = Path.Combine(path, folder);
            }

            path = Path.Combine(path, fileName);

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return fileName;
        }

        public static bool CheckString(this string str)
        {
            foreach (char item in str)
            {
                if (!char.IsLetter(item))
                {
                    return true;
                }
            }

            return false;
        }
        //public static bool CheckNumber(this string num)
        //{

        //    bool isParsable = Int32.TryParse(num, out number);
        //}
    }
}
