using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Helpers;
using Web.Models;

namespace Web.Controllers
{
    public class RootController : Controller
    {
        public Func<string> GetImagesPath;
        public Func<string,FileManager> GetFileManger; 
     
        public RootController()
        {
            GetImagesPath = () => Server.MapPath("~/Content/Images");
            GetFileManger = path => new FileManager(path); 
        }

        public ActionResult Index()
        {
            var images = getFileManager().GetProcessed();
            return View(images);
        }

        [HttpPost, JsonError]
        public JsonResult Upload(HttpPostedFileBase file)
        {
            var fileId = getFileManager().SaveForProcessing(file);

            return Json(new { FileId = fileId.ToString() }, "text/plain", System.Text.Encoding.UTF8);
        }

        [JsonError]
        public ActionResult ImageStatus(string fileId)
        {
            var image = getFileManager().HasProcessed(fileId);
            return Json(image, JsonRequestBehavior.AllowGet);
        }
        
        private FileManager getFileManager()
        {
            return GetFileManger(GetImagesPath());
        }
    }
}
