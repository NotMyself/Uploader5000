using System;
using System.Web.Mvc;
using Web.Models;

namespace Web.Controllers
{
    public class ImagesController : Controller
    {
        public ImageProcessor Processor { get; set; }

        public Func<string> GetImagesPath; 

        public ImagesController()
        {
            Processor = new ImageProcessor();
            GetImagesPath = () => Server.MapPath("~/Content/Images");
        }

        public ActionResult Process()
        {
            Processor.Process(GetImagesPath());
            
            return new HttpStatusCodeResult(200);
        }

    }
}
