using System.IO;
using System.Linq;

namespace Web.Models
{
    public class ImageProcessor
    {
        public virtual void Process(string path)
        {
            var sourcePath = Path.Combine(path, "Processing");
            var destinationPath = Path.Combine(path, "Processed");
            foreach (var directory in Directory.GetDirectories(sourcePath).Take(5))
            {
                var fileId = directory.Split(Path.DirectorySeparatorChar).Last();
                Directory.Move(directory, Path.Combine(destinationPath, fileId));
            }
        }
    }
}