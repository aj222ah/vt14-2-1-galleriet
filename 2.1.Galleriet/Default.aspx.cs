using _2._1.Galleriet.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _2._1.Galleriet
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void UploadPhoto_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
               /* if (ChooseImage.HasFile)
                {
                    var fileName = ChooseImage.FileName;
                }
                */
            }
        }

        // Hämta filer från folder och returnera relevant värde
        public IEnumerable<dynamic> Repeater_GetData()
        {
            var di = new DirectoryInfo(Server.MapPath("~/Content/images/thumbnails"));

            return (from fi in di.GetFiles()
                    select new Photo
                    {
                        Name = fi.Name,
                    }).AsEnumerable();
        }

        // The id parameter should match the DataKeyNames value set on the control
        // or be decorated with a value provider attribute, e.g. [QueryString]int id
        /*public _2._1.Galleriet.Model.Photo PhotoCloseUp_GetItem(int id)
        {
            return null;
        }*/
    }
}