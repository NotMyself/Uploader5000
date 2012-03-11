using Web.Models;

namespace Test.Helpers.Fakes
{
    public class FakePollingFileManager : FileManager
    {
        public FakePollingFileManager(string path):base(path)
        {
            RootImagesPath = path;
        }

        public ProcessedImageResult Result { get; set; }
        public string FileId { get; set; }
        public string RootImagesPath { get; set; }
        
        public override ProcessedImageResult HasProcessed(string fileId)
        {
            FileId = fileId;
            return Result = new ProcessedImageResult{ IsFinished = true, ImagePath = "somepath"};
        }
    }
}