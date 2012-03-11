using Web.Models;

namespace Test.Helpers.Fakes
{
    public class FakeImageProcessor : ImageProcessor
    {
        public string ProcessedPath { get; set; }

        public override void Process(string path)
        {
            ProcessedPath = path;
        }
    }
}