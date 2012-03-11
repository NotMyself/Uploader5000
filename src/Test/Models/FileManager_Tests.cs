using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Shouldly;
using Test.Helpers;
using Test.Helpers.Fakes;
using Web.Models;

namespace Test.Models
{
    [TestFixture]
    public class when_I_get_processed_images : SpecFor<FileManager>
    {
        private string processedImagesPath;
        private IEnumerable<ProcessedImageResult> result;

        public override void Context()
        {
            processedImagesPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Images");
            subject = new FileManager(processedImagesPath);
        }

        public override void Because()
        {
            result = subject.GetProcessed();
        }

        [Test]
        public void it_should_return_the_results()
        {
            result.Count().ShouldBe(3);
        }

        [Test]
        public void it_should_mark_the_results_as_processed()
        {
            result.Any(x => !x.IsFinished).ShouldBe(false);
        }

        [Test]
        public void it_should_include_paths_for_the_images()
        {
            result.Any(x => string.IsNullOrWhiteSpace(x.ImagePath)).ShouldBe(false);
        }
    }

    [TestFixture]
    public class when_I_check_if_an_image_that_has_been_processed_has_processed : SpecFor<FileManager>
    {
        private string processedImagesPath;
        private string fileId;
        private ProcessedImageResult result;

        public override void Context()
        {
            processedImagesPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Images");
            fileId = "46b9c878-e7e0-4f80-a441-c93694b0c210";
            subject = new FileManager(processedImagesPath);
        }

        public override void Because()
        {
            result = subject.HasProcessed(fileId);
        }

        [Test]
        public void it_should_indicate_that_the_file_has_processed()
        {
            result.IsFinished.ShouldBe(true);
        }

        [Test]
        public void it_should_return_the_path_the_file()
        {
            result.ImagePath.ShouldContain(fileId);
        }
    }

    [TestFixture]
    public class when_I_check_if_an_image_that_has_not_been_processed_has_processed : SpecFor<FileManager>
    {
        private string processedImagesPath;
        private string fileId;
        private ProcessedImageResult result;

        public override void Context()
        {
            processedImagesPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Images");
            fileId = "some-unprocessed-file-id";
            subject = new FileManager(processedImagesPath);
        }

        public override void Because()
        {
            result = subject.HasProcessed(fileId);
        }

        [Test]
        public void it_should_indicate_that_the_file_has_not_processed()
        {
            result.IsFinished.ShouldBe(false);
        }

        [Test]
        public void it_should_return_the_path_the_file()
        {
            result.ImagePath.ShouldBeEmpty();
        }
    }

    [TestFixture]
    public class when_I_save_a_file_for_procesing : SpecFor<FileManager>
    {
        private FakeHttpPostedFile file;
        private string processedImagesPath;
        private Guid result;

        public override void Context()
        {
            processedImagesPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Images");
            file = new FakeHttpPostedFile("someimagefile.png");
            subject = new FileManager(processedImagesPath);
        }

        public override void Because()
        {
            result = subject.SaveForProcessing(file);
        }

        [Test]
        public void it_should_return_the_file_id()
        {
            result.ShouldNotBe(Guid.Empty);
        }

        [Test]
        public void it_should_create_a_folder_for_the_image()
        {
            Directory.Exists(Path.Combine(processedImagesPath, FileManager.ProcessingFolder, result.ToString())).ShouldBe(true);
        }

        [Test]
        public void it_should_save_the_file_to_the_processing_folder()
        {
            file.SavedAsPath.ShouldBe(Path.Combine(processedImagesPath,FileManager.ProcessingFolder, result.ToString(), file.FileName));
        }
    }
}
