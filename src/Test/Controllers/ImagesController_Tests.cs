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
    public class when_I_process_images : SpecFor<ImagesController>
    {
        private string path;
        private FakeImageProcessor fakeProcessor;
        private ActionResult result;

        public override void Context()
        {
            path = "C:\\somepath\\somewhere";
            fakeProcessor = new FakeImageProcessor();
            subject = new ImagesController
            {
                GetImagesPath = () => path,
                Processor = fakeProcessor
            };    
        }

        public override void Because()
        {
            result = subject.Process();
        }

        [Test]
        public void it_should_process_the_path()
        {
            fakeProcessor.ProcessedPath.ShouldBe(path);
        }

        [Test]
        public void it_should_indicate_it_was_successful()
        {
            result.ShouldBeTypeOf<HttpStatusCodeResult>();
            ((HttpStatusCodeResult)result).StatusCode.ShouldBe(200);
        }
    }
}
