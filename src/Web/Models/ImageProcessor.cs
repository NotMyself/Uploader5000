using System.IO;
using System.Linq;
using ImageResizer;

namespace Web.Models
{
    public class ImageProcessor
    {
        public virtual void Process(string path)
        {
            var sourcePath = Path.Combine(path, ProcessPaths.ProcessingFolder);
            var destinationPath = Path.Combine(path, ProcessPaths.ProcessedFolder);
            foreach (var directory in Directory.GetDirectories(sourcePath))
            {
                var fileId = directory.Split(Path.DirectorySeparatorChar).Last();
                var file = Directory.GetFiles(directory).Single();
                var fileName = Path.GetFileName(file);
                var destinationFile = Path.Combine(destinationPath, fileId, fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(destinationFile));
                
                ImageBuilder.Current.Build(file, destinationFile,
                                                     new ResizeSettings{MaxHeight = 700, MaxWidth = 500});
                Directory.Delete(directory, true);
            }
        }
    }
}