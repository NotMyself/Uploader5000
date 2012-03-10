using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        [Test]
        public void site_root_should_route_to_root_controller_index_action()
        {
            "~/".ShouldMapTo<RootController>(x => x.Index());
        }
    }
}
