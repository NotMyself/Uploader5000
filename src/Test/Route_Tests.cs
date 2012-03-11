using System.Web.Routing;
using NUnit.Framework;
using Test.Helpers;
using Web;
using Web.Controllers;

namespace Test
{
    [TestFixture]
    public class Route_Tests
    {
        [SetUp]
        public void Setup()
        {
            MvcApplication.RegisterRoutes(RouteTable.Routes);
        }
        [TearDown]
        public void TearDown()
        {
            RouteTable.Routes.Clear();
        }
        
        [Test]
        public void site_root_should_route_to_root_controller_index_action()
        {
            "~/".ShouldMapTo<RootController>(x => x.Index());
        }

        [Test]
        public void upload_should_route_to_root_controller_upload_action()
        {
            "~/Upload".ShouldMapTo<RootController>(x => x.Upload(null));
        }

        [Test]
        public void image_should_route_to_root_controller_image_action()
        {
            "~/Image/some-file-id".ShouldMapTo<RootController>(x => x.Image("some-file-id"));
        }
    }
}
