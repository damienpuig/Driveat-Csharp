using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Driveat.Services.ImageService
{
    /// <summary>
    /// Image Service layer, containing the business of the application according to dishes.
    /// Each method accept data object.
    /// In a Real application, the use of mappers such as http://automapper.org/ is needed in order to perform transparency between layers.
    /// An object isn't shared between layers, but is mapped from Presentation view model to Data transfert object or command, and persisted to 
    /// the database as real entity objects.
    /// Dish types cannot be created in this coursework scope, because dish type are specific to the business and have to be defined only by
    /// administrators.
    /// </summary>
    public interface IImageService
    {
        /// <summary>
        /// Save an image to the server.
        /// The save image only save images on the server and doesn't save images to GridFS database ( http://www.mongodb.org/display/DOCS/GridFS+Specification )
        /// </summary>
        /// <param name="file">file posted as a HttpPostedFileBase supported by the .NET framework, containing image specification</param>
        /// <param name="tag">the tag characterize an image, by the user id.</param>
        /// <param name="folderpath">folder used by the presentation to save images.</param>
        /// <returns>return the picture filname saved.</returns>
         string SaveImage(HttpPostedFileBase file, string tag, string folderpath);

        /// <summary>
        /// Update the user picture to the server.
        /// The update performs a check if the file already exists ( wrong : the user can provide another picture with the same filename.)
        /// and update the file.
        /// Based on tweeter example, the user replacing the picture cannot retreive the old one.
        /// </summary>
        /// <param name="file">file given to update</param>
        /// <param name="tag">the tag characterize an image, by the user id.</param>
        /// <param name="folderpath">folder used by the presentation to save images.</param>
        /// <param name="oldfilename">current file used by the used.</param>
        /// <returns></returns>
         string UpdateImage(HttpPostedFileBase file, string tag, string folderpath, string oldfilename);
            
           
    }
}
