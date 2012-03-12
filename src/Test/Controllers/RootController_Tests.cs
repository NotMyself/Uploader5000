using System;
using System.Web.Mvc;
using NUnit.Framework;
using Shouldly;
using Test.Helpers;
using Test.Helpers.Fakes;
using Web.Controllers;

namespace Test.Controllers
{
    // ReSharper disable InconsistentNaming

    [TestFixture]
    public class when_I_view_the_index_page : SpecFor<RootController>
    {
        private ActionResult result;
        private string path;
        private FakeQueryFileManager fileManager;

        public override void Context()
        {
            path = "C:\\somepath\\somewhere";
            subject = new RootController
            {
                GetImagesPath = () => path,
                GetFileManger = s => fileManager = new FakeQueryFileManager(s)
            };
        }

        public override void Because()
        {
            result = subject.Index();
        }

        [Test]
        public void it_should_get_a_file_manager_for_the_path()
        {
            fileManager.RootImagesPath.ShouldBe(path);
        }

        [Test]
        public void it_should_return_the_result()
        {
            result.ShouldBeTypeOf<ViewResult>();
            ((ViewResult)result).Model.ShouldBe(fileManager.ProcessedImages);
        }
    }

    [TestFixture]
    public class when_I_upload_an_image : SpecFor<RootController>
    {
        private FakeHttpPostedFile file;
        private ActionResult result;
        private string path;
        private FakeUploadFileManager fileManager;

        public override void Context()
        {
            path = "C:\\somepath\\somewhere";
            file = new FakeHttpPostedFile("somefile.png");
            subject = new RootController
                          {
                              GetImagesPath = () => path,
                              GetFileManger = s => fileManager = new FakeUploadFileManager(s)
                          };
        }

        public override void Because()
        {
            result = subject.Upload(file);
        }

        [Test]
        public void it_should_get_a_file_manager_for_the_path()
        {
            fileManager.RootImagesPath.ShouldBe(path);
        }

        [Test]
        public void it_should_save_the_file()
        {
            fileManager.SavedFile.ShouldBe(file);
        }

        [Test]
        public void it_should_return_the_file_id()
        {
            result.ShouldBeTypeOf<JsonResult>();
        }
    }

    [TestFixture]
    public class when_I_check_the_status_of_an_image : SpecFor<RootController>
    {
        private ActionResult result;
        private string path;
        private FakePollingFileManager pollingFileManager;
        private string fileId;

        public override void Context()
        {
            fileId = Guid.NewGuid().ToString();
            path = "C:\\somepath\\somewhere";
            subject = new RootController
            {
                GetImagesPath = () => path,
                GetFileManger = s => pollingFileManager = new FakePollingFileManager(s)
            };
        }

        public override void Because()
        {
            result = subject.ImageStatus(fileId);
        }

        [Test]
        public void it_should_get_a_file_manager_for_the_path()
        {
            pollingFileManager.RootImagesPath.ShouldBe(path);
        }

        [Test]
        public void it_should_query_for_the_file_by_id()
        {
            pollingFileManager.FileId.ShouldBe(fileId);
        }

        [Test]
        public void it_should_return_the_result()
        {
            result.ShouldBeTypeOf<JsonResult>();
            ((JsonResult)result).Data.ShouldBe(pollingFileManager.Result);
        }
    }
}
