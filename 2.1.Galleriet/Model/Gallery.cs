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


        static Gallery()
        {
            string extensionsString = @"^.*\.(jpg|jpeg|png|gif)$";
            var invalidChars = new string(Path.GetInvalidFileNameChars());
            SanitizePath = new Regex(string.Format("[{0}]", Regex.Escape(invalidChars)));
            ApprovedExtension = new Regex(extensionsString, RegexOptions.IgnoreCase);
        }

        public string SaveImage(Stream stream, string fileName)
        {
            string imageFileName = "";
            string thumbnailFileName = "";
            Image thumbnail;

            // Kolla så att filen har en godkänd filändelse
            if (ApprovedExtension.IsMatch(fileName))
            {
                // Skapa nytt Image-objekt
                Image newImage = Image.FromStream(stream);

                // Kolla om Image-objektet är rätt MIME-typ, kasta undantag om fel MIME-typ
                if (!IsValidImage(newImage))
                {
                    throw new ArgumentException("Filen du har laddat upp är inte ett godkänt bildformat.");
                }

                fileName = SanitizePath.Replace(fileName, "");

                
                // Kolla om det angivna, rensade filnamnet redan finns annars redigera det
                if (ImageExists(fileName))
                {
                    string nameWithoutExtension;
                    string extension; ;
                    string tempFileName = fileName;
                    do
                    {
                        int divider = tempFileName.LastIndexOf('.');
                        nameWithoutExtension = tempFileName.Substring(0, divider);
                        extension = tempFileName.Substring(divider, tempFileName.Length - divider);
                        Regex findParenthesis = new Regex(@"\((.*)\)");
                        int imageNumber;

                        if (findParenthesis.IsMatch(nameWithoutExtension))
                        {
                            // Hitta index från parentes ( och ) och korrigera till plats efter respektive före
                            int firstCharOfSubstring = nameWithoutExtension.LastIndexOf('(') + 1;
                            int lastCharOfSubstring = nameWithoutExtension.LastIndexOf(')') - 1;
                            int substringLength = firstCharOfSubstring - lastCharOfSubstring;

                            // Kolla om substringen kan tolkas som ett heltal, öka i så fall på med 1
                            if (int.TryParse(nameWithoutExtension.Substring(firstCharOfSubstring, substringLength), out imageNumber))
                            {
                                string tempString = "";
                                imageNumber = int.Parse(nameWithoutExtension.Substring(firstCharOfSubstring, substringLength));
                                imageNumber++;
                                tempString = nameWithoutExtension.Remove(firstCharOfSubstring, substringLength).Insert(firstCharOfSubstring, imageNumber.ToString());
                                nameWithoutExtension = tempString;
                            }
                            else
                            {
                                nameWithoutExtension += "(1)";
                            }

                        }
                        else
                        {
                            nameWithoutExtension += "(1)";
                        }
                        tempFileName = nameWithoutExtension + extension;
                    } while (ImageExists(tempFileName));

                    fileName = nameWithoutExtension + extension;
                }

                imageFileName = fileName;
                thumbnailFileName = "thumb" + imageFileName;

                thumbnail = newImage.GetThumbnailImage(75, 100, null, System.IntPtr.Zero);
                thumbnail.Save(Path.Combine(PhysicalUploadedImagesPath, @"\thumbnails\", thumbnailFileName));
                newImage.Save(Path.Combine(PhysicalUploadedImagesPath, @"\", imageFileName));

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

        // Metod som kontrollerar om den skapade bilden är av godkänt format
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