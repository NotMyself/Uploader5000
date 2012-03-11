using System.Collections.Generic;
using System.Linq;
using Web.Models;

namespace Test.Helpers.Fakes
{
    public class FakeQueryFileManager : FileManager
    {
        public FakeQueryFileManager(string path) : base(path)
        {
            RootImagesPath = path;
            ProcessedImages = Enumerable.Empty<ProcessedImageResult>();
        }
        public string RootImagesPath { get; set; }
        public IEnumerable<ProcessedImageResult> ProcessedImages { get; set; }

        public override IEnumerable<ProcessedImageResult> GetProcessed()
        {
            return ProcessedImages;
        }
    }
}