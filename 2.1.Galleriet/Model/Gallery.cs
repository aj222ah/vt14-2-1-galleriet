using System;
using System.Collections.Generic;
using System.Drawing;
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
        private static string PhysicalUploadedImagesPath { get { return Path.Combine(AppDomain.CurrentDomain.GetData("APPBASE").ToString(), @"\Content\images"); } }


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

            // Kolla så att filen har en godkänd filändelse
            if (ApprovedExtension.IsMatch(fileName))
            {
                // Skapa nytt Image-objekt
                Image newImage = Image.FromFile(fileName);

                // Kolla om Image-objektet är rätt MIME-typ, kasta undantag om fel MIME-typ
                if (!IsValidImage(newImage))
                {
                    throw new ArgumentException("Filen du har laddat upp är inte ett godkänt bildformat.");
                }

                fileName = SanitizePath.Replace(fileName, "");

                
                // Kolla om det angivna, rensade filnamnet redan finns annars redigera det
                if (ImageExists(fileName))
                {
                    string tempFileName = fileName;
                    do
                    {
                        int divider = tempFileName.LastIndexOf('.');
                        string nameWithoutExtension = tempFileName.Substring(0, divider);
                        string extension = tempFileName.Substring(divider, tempFileName.Length - divider);

                        if (nameWithoutExtension.LastIndexOf(')') == (nameWithoutExtension.Length - 1) &&
                                nameWithoutExtension.LastIndexOf('(') == (nameWithoutExtension.Length - 3))
                        {
                            int index = nameWithoutExtension.LastIndexOf(')') - 1;
                            int number = int.Parse(nameWithoutExtension[index].ToString());
                        }
                        else
                        {
                            nameWithoutExtension += "(1)";
                        }

                        tempFileName = nameWithoutExtension + extension;
                    } while (ImageExists(tempFileName));
                }



                return imageFileName;
            }
            else
            {
                throw new ArgumentException("Filnamnet du har angett är inte ett godkänt bildformat.");
            }
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

        // Metod som kollar om bildnamnet redan finns
        public static bool ImageExists(string name)
        {
            Gallery temp = new Gallery();
            IEnumerable<string> existingNames = temp.GetImageNames();
            foreach (string str in existingNames)
            {
                if (str == name)
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsValidImage(Image image)
        {
            if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Jpeg.Guid)
            {
                return true;
            }
            else if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Gif.Guid)
            {
                return true;
            }
            else if (image.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Png.Guid)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}