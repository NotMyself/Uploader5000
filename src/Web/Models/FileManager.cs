using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Web.Models
{
    public class ProcessedImageResult
    {
        public bool IsFinished { get; set; }
        public string ImagePath { get; set; }
    }

    public class FileManager
    {
        private readonly string processedPath;
        private readonly string processingPath;

        public FileManager(string path)
        {   
            processedPath = Path.Combine(path, ProcessPaths.ProcessedFolder);
            processingPath = Path.Combine(path, ProcessPaths.ProcessingFolder);
        }

        public virtual Guid SaveForProcessing(HttpPostedFileBase file)
        {
            var fileId = Guid.NewGuid();
            var fileName = Path.GetFileName(file.FileName);
            var path = Path.Combine(processingPath, fileId.ToString());
            
            Directory.CreateDirectory(path);
            file.SaveAs(Path.Combine(path, fileName));

            return fileId;
        }

        public virtual ProcessedImageResult HasProcessed(string fileId)
        {
            var results = from directory in Directory.GetDirectories(processedPath)
                          where directory.EndsWith(fileId)
                          let file = Directory.GetFiles(directory).Single().Replace(processedPath, string.Empty)
                          select new ProcessedImageResult
                                     {
                                         IsFinished = true,
                                         ImagePath = string.Format("/Content/Images/Processed/{0}", file)
                                     };
            return results.SingleOrDefault() ?? new ProcessedImageResult { ImagePath = string.Empty };
        }

        public virtual IEnumerable<ProcessedImageResult> GetProcessed()
        {
            return from directory in Directory.GetDirectories(processedPath)
                   let file = Directory.GetFiles(directory).Single().Replace(processedPath, string.Empty)
                      select new ProcessedImageResult
                                 {
                                     IsFinished = true,
                                     ImagePath = string.Format("/Content/Images/Processed/{0}", file)
                                 };
        }
    }
}