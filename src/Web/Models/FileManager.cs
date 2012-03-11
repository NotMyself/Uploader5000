using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Web.Controllers;

namespace Web.Models
{
    public class ProcessedImageResult
    {
        public bool IsFinished { get; set; }
        public string ImagePath { get; set; }
    }

    public class FileManager
    {
        private readonly string path;

        public FileManager(string path)
        {
            this.path = path;
        }

        public virtual Guid SaveForProcessing(HttpPostedFileBase file)
        {
            var fileName = Path.GetFileName(file.FileName);
            var fileId = Guid.NewGuid();
            var path = Path.Combine(Path.Combine(this.path, "Processing"), fileId.ToString());
            Directory.CreateDirectory(path);
            file.SaveAs(Path.Combine(path, fileName));

            return fileId;
        }

        public virtual ProcessedImageResult HasProcessed(string fileId)
        {
            var path = Path.Combine(this.path, "Processed");
            var directories = Directory.GetDirectories(path);
            if (directories.Any(x => x.EndsWith(fileId)))
            {
                var file = Directory.GetFiles(directories.First(x => x.EndsWith(fileId))).Single();
                path = string.Format("/Content/Images/Processed/{0}/{1}", fileId, Path.GetFileName(file));
            }
            return new ProcessedImageResult
            {
                IsFinished = directories.Any(x => x.EndsWith(fileId)),
                ImagePath = path
            };
        }

        public virtual IEnumerable<ProcessedImageResult> GetProcessed()
        {
            return new[] {new ProcessedImageResult
                              {
                                  IsFinished = true, 
                                  ImagePath = "d63ab745-4278-4f2b-942a-55cba8a3e4cf/Fruit_F.png"
                              }};
        }
    }
}