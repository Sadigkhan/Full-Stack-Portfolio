using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace DekorEvStartUpFinal.Helper
{
    public class Helper
    {
        public static void DeleteFile(IWebHostEnvironment env,string fileName,params string[] folders)
        {
            string path = env.WebRootPath;
            foreach (string folder in folders)
            {
                path=Path.Combine(path, folder);
            }


            path = Path.Combine(path, fileName);


            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
