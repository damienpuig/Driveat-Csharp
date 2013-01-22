using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driveat.Services.ImageService
{
   public class ImageService :IImageService
    {

       // The save image implementation provided by the service uses a private method shared by both method implementations. 
        public string SaveImage(System.Web.HttpPostedFileBase file, string tag, string folderpath)
        {
           return save(file, tag, folderpath);
        }

       // Check if the filename exists, and delete it if needed.
        public string UpdateImage(System.Web.HttpPostedFileBase file, string tag, string folderpath, string oldfilename)
        {

            if (!string.IsNullOrEmpty(oldfilename) && file != null)
            {
                var path = Path.Combine(folderpath, oldfilename);

                if (File.Exists(path))
                    File.Delete(path);

            }

            return save(file, tag, folderpath);
        }


       // Shared private method in order to save a file cheecking if the content is present. 
       // If there is content, the filename is saved.
        private string save(System.Web.HttpPostedFileBase file, string tag, string folderpath)
        {
            if (file != null && file.ContentLength > 0)
            {

                var fileName = string.Format("{0}_{1}", tag, Path.GetFileName(file.FileName));

                var path = Path.Combine(folderpath, fileName);

                file.SaveAs(path);

                return fileName;
            }
            return null;
        }
    }
}
