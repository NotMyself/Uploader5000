using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Shouldly;
using NUnit.Framework;
using Test.Helpers;
using Web.Helpers;
using Web.Models;

namespace Test.Models
{
    // ReSharper disable InconsistentNaming
    
    public class Demensions
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
    
    [TestFixture]
    public abstract class with_an_image_processor : SpecFor<ImageProcessor>
    {
        protected string imagesPath;
        protected string processedDirectory;
        protected string processingDirectory;
        protected string originalDirectory;
        protected string startDirectory;
        protected string fileId;
        protected string fileName;

        public override void Context()
        {
            imagesPath = Path.Combine(TestContext.CurrentContext.TestDirectory, GetImagePath());
            processedDirectory = Path.Combine(imagesPath, ProcessPaths.ProcessedFolder);
            processingDirectory = Path.Combine(imagesPath, ProcessPaths.ProcessingFolder);
            originalDirectory = Path.Combine(imagesPath, ProcessPaths.OriginalDirectory);
            startDirectory = Path.Combine(imagesPath, "Start");
            CreateOrClean(processedDirectory); 
            CreateOrClean(originalDirectory);
            initProcessingDir();
        }

        private void initProcessingDir()
        {
            CreateOrClean(processingDirectory);
            Directory.GetDirectories(startDirectory)
                     .Each(x =>
                               {
                                   var destination = Path.Combine(processingDirectory,
                                                                  x.Split(Path.DirectorySeparatorChar).Last());
                                   Directory.CreateDirectory(destination);
                                   Directory.GetFiles(x).Each(z => File.Copy(z,
                                       Path.Combine(destination, z.Split(Path.DirectorySeparatorChar).Last())));

                               });
        }

        private void CreateOrClean(string directory)
        {
            if (Directory.Exists(directory))
                Directory.GetDirectories(directory).Each(x => Directory.Delete(x, true));
            else
                Directory.CreateDirectory(directory);
        }

        public abstract string GetImagePath();

        protected Demensions GetProcessedImageDemensions()
        {
            var directory = Directory.GetDirectories(processedDirectory).Single();
            var file = Directory.GetFiles(directory).Single();
            using (var image = Image.FromFile(file))
            {
                return new Demensions { Width = image.Width, Height = image.Height };
            }
        }

        [Test]
        public void it_should_move_the_processed_image_to_the_processed_folder()
        {
            File.Exists(Path.Combine(processedDirectory, fileId, fileName))
                .ShouldBe(true);
        }

        [Test]
        public void it_should_remove_the_processing_folder()
        {
            File.Exists(Path.Combine(processingDirectory, fileId, fileName))
                .ShouldBe(false);
        }
    }

    [TestFixture]
    public class when_I_process_an_image_that_is_ok : with_an_image_processor
    {
        public override string GetImagePath()
        {
            return "Images\\ImageProcessor\\ImageOk";
        }

        public override void Context()
        {
           base.Context();
            fileId = "46b9c878-e7e0-4f80-a441-c93694b0c212";
            fileName = "ImageOk.jpg";
            subject = new ImageProcessor();
        }

        public override void Because()
        {
            subject.Process(imagesPath);
        }

        [Test, Ignore]
        public void it_should_not_move_the_image_to_the_original_folder()
        {
            Directory.GetDirectories(originalDirectory).ShouldBeEmpty();
        }

        [Test]
        public void it_should_not_resize_the_image()
        {
            var imageDemensions = GetProcessedImageDemensions();
            imageDemensions.Height.ShouldBe(601);
            imageDemensions.Width.ShouldBe(400);
        }
    }

    [TestFixture]
    public class when_I_process_an_image_that_is_to_tall : with_an_image_processor
    {
        public override string GetImagePath()
        {
            return "Images\\ImageProcessor\\ImageToTall";
        }

        public override void Context()
        {
            base.Context();
            fileId = "46b9c878-e7e0-4f80-a441-c93694b0c212";
            fileName = "ImageToTall.jpg";
            subject = new ImageProcessor();
        }

        public override void Because()
        {
            subject.Process(imagesPath);
        }

        [Test, Ignore]
        public void it_should_move_the_image_to_the_original_folder()
        {
            Directory.GetDirectories(originalDirectory).Count().ShouldBe(1);
        }

        [Test]
        public void it_should_resize_the_image()
        {
            var imageDemensions = GetProcessedImageDemensions();
            imageDemensions.Height.ShouldBeLessThan(700);
            imageDemensions.Width.ShouldBeLessThan(501);
        }
    }

    [TestFixture]
    public class when_I_process_an_image_that_is_to_wide : with_an_image_processor
    {
        public override string GetImagePath()
        {
            return "Images\\ImageProcessor\\ImageToWide";
        }

        public override void Context()
        {
            base.Context();
            fileId = "46b9c878-e7e0-4f80-a441-c93694b0c212";
            fileName = "ImageToWide.jpg";
            subject = new ImageProcessor();
        }

        public override void Because()
        {
            subject.Process(imagesPath);
        }

        [Test, Ignore]
        public void it_should_move_the_image_to_the_original_folder()
        {
            Directory.GetDirectories(originalDirectory).Count().ShouldBe(1);
        }

        [Test]
        public void it_should_resize_the_image()
        {
            var imageDemensions = GetProcessedImageDemensions();
            imageDemensions.Height.ShouldBeLessThan(700);
            imageDemensions.Width.ShouldBeLessThan(501);
        }
    }
}
