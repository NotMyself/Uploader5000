using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class RootController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            var fileName = Path.GetFileName(file.FileName);
            var fileId = Guid.NewGuid().ToString();
            var path = Path.Combine(Server.MapPath("~/Content/Images/Processing"),fileId);
            Directory.CreateDirectory(path);
            file.SaveAs(Path.Combine(path, fileName));
            
            return new JsonResult {Data = new {fileId = fileId}};
        }

        
        public ActionResult Image(string fileId)
        {
            var path = Server.MapPath("~/Content/Images/Processing");
            var directories = Directory.GetDirectories(path);
            if (directories.Any(x => x.EndsWith(fileId)))
            {
                var file = Directory.GetFiles(directories.First(x => x.EndsWith(fileId))).Single();
                path = string.Format("/Content/Images/Processing/{0}/{1}", fileId, Path.GetFileName(file));
            }
            var imageResult = new ImagePollingResult
                                  {
                                      IsFinished = directories.Any(x => x.EndsWith(fileId)),
                                      ImagePath = path
                                  };
            return Json(imageResult, JsonRequestBehavior.AllowGet);
        }
    }

    class ImagePollingResult
    {
        public bool IsFinished { get; set; }
        public string ImagePath { get; set; }
    }
}
