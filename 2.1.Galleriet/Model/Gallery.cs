using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace _2._1.Galleriet.Model
{
    public class Gallery
    {
        private static Regex ApprovedExtension { get; set; }
        private static Regex SanitizePath { get; set; }
        private static string PhysicalUploadedImagesPath { get { return AppDomain.CurrentDomain.GetData("APPBASE").ToString(); } }


        public static Gallery()
        {
            string extensionsString = @"^.*\.(jpg|jpeg|png|gif)$";
            var invalidChars = new string(Path.GetInvalidFileNameChars());
            SanitizePath = new Regex(string.Format("[{0}]", Regex.Escape(invalidChars)));
            ApprovedExtension = new Regex(extensionsString, RegexOptions.IgnoreCase);
        }

        public string SaveImage(Stream stream, string fileName)
        {
            string imageFileName = "";



            return imageFileName;
        }

        public IEnumerable<string> GetImageNames()
        {
            List<string> imageNames = new List<string>();

            // Skapa DirectoryInfo-objekt och hämta filer
            var di = new DirectoryInfo(PhysicalUploadedImagesPath);
            var files = di.GetFiles();

            // Loopa igenom filerna, kolla om relevant filändelse och lägg i så fall till fil i lista
            for (int i = 0; i < files.Length; i++ )
            {
                if (ApprovedExtension.IsMatch(Path.GetFileName(files[i].ToString())))
                {
                    imageNames.Add(Path.GetFileName(files[i].ToString()));
                }
            }

            return imageNames.AsReadOnly();
        }
    }
}