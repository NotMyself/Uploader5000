using System;
using System.Web;
using Web.Models;

namespace Test.Helpers.Fakes
{
    public class FakeUploadFileManager : FileManager
    {
        public FakeUploadFileManager(string path) : base(path)
        {
            RootImagesPath = path;
        }

        public string RootImagesPath { get; set; }
        public HttpPostedFileBase SavedFile { get; set; }
        public Guid SavedFileId { get; set; }

        public override Guid SaveForProcessing(HttpPostedFileBase file)
        {
            SavedFileId = Guid.NewGuid();
            SavedFile = file;

            return SavedFileId;
        }

    }
}