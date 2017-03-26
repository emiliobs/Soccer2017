using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Backend.Helpers
{
    public class FileHelper
    {
        public static string UploadPhoto(HttpPostedFileBase file, string folder)
        {
            string path = string.Empty;
            string picture = string.Empty;

            if (file != null)
            {
                picture = Path.GetFileName(file.FileName);
                path = Path.Combine(HttpContext.Current.Server.MapPath(folder), picture);
                file.SaveAs(path);
            }

            return picture;
        }
    }
}