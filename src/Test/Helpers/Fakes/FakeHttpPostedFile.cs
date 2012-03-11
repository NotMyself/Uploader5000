using System.Web;

namespace Test.Helpers.Fakes
{
    public class FakeHttpPostedFile : HttpPostedFileBase
    {
        private readonly string fileName;

        public FakeHttpPostedFile(string fileName)
        {
            this.fileName = fileName;
        }

        public string SavedAsPath { get; set; }
        public override string FileName{ get { return fileName; } }
        public override void SaveAs(string filename)
        {
           SavedAsPath = filename;
        }
    }
}