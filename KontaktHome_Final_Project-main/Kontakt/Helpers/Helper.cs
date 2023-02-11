using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt.Helpers
{
    public class Helper
    {
        public static void DeleteFile(IWebHostEnvironment env,string fileName, params string[] folders)
        {
            string path = env.WebRootPath;

            foreach (string folder in folders)
            {
                path = Path.Combine(path, folder);
            }

            path = Path.Combine(path, fileName);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        public enum Roles
        {
            Admin,
            Maneger,
            Member,
            Publisher

        }
        public enum Gender
        {
            Kişi,
            Qadın,

        }
        public enum OrderStatus
        {
            Gözləyən,
            Təsdiqlənən,
            İmtina,
            Çatdırılan
        }
    }
}
