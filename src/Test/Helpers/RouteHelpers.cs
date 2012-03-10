using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Shouldly;


//adapted from MVC Contrib
//http://mvccontrib.codeplex.com/SourceControl/changeset/view/eced037865f2#src%2fMvcContrib.TestHelper%2fMvcContrib.TestHelper%2fRouteTestingExtensions.cs
namespace Test.Helpers
{

    /// <summary>
    /// Used to simplify testing routes.
    /// </summary>
    public static class RouteTestingExtensions
    {
        public class FakeHttpContext : HttpContextBase
        {
            private readonly HttpRequestBase request;

            public FakeHttpContext(HttpRequestBase request)
            {
                this.request = request;
            }

            public override HttpRequestBase Request
            {
                get { return request; }
            }
        }
        public class FakeHttpRequest : HttpRequestBase
        {
            private readonly string url;

            public FakeHttpRequest(string url)
            {
                this.url = url;
            }

            public override string AppRelativeCurrentExecutionFilePath
            {
                get { return url; }
            }

            public override string PathInfo
            {
                get { return string.Empty; }
            }
        }
        
        private static HttpContextBase BuildFakeHttpContext(string url)
        {
            var request = new FakeHttpRequest(url);
            var context = new FakeHttpContext(request);
            return context;
        }

        /// <summary>
        /// Returns the corresponding route for the URL.  Returns null if no route was found.
        /// </summary>
        /// <param name="url">The app relative url to test.</param>
        /// <returns>A matching <see cref="RouteData" />, or null.</returns>
        public static RouteData Route(this string url)
        {
            var context = BuildFakeHttpContext(url);
            return RouteTable.Routes.GetRouteData(context);
        }

        /// <summary>
        /// Asserts that the route matches the expression specified.  Checks controller, action, and any method arguments
        /// into the action as route values.
        /// </summary>
        /// <typeparam name="TController">The controller.</typeparam>
        /// <param name="routeData">The routeData to check</param>
        /// <param name="action">The action to call on TController.</param>
        public static RouteData ShouldMapTo<TController>(this RouteData routeData, Expression<Func<TController, ActionResult>> action)
            where TController : IController
        {
            routeData.ShouldNotBe(null);

            //check controller
            routeData.ShouldMapTo<TController>();

            //check action
            var methodCall = (MethodCallExpression)action.Body;
            string actualAction = routeData.Values.GetValue("action").ToString();
            string expectedAction = methodCall.Method.Name;
            actualAction.ShouldBe(expectedAction);

            //check parameters
            for (int i = 0; i < methodCall.Arguments.Count; i++)
            {
                string name = methodCall.Method.GetParameters()[i].Name;
                object value = null;

                switch (methodCall.Arguments[i].NodeType)
                {
                    case ExpressionType.Constant:
                        value = ((ConstantExpression)methodCall.Arguments[i]).Value;
                        break;

                    case ExpressionType.MemberAccess:
                        value = Expression.Lambda(methodCall.Arguments[i]).Compile().DynamicInvoke();
                        break;

                }

                value = (value == null ? value : value.ToString());
                routeData.Values.GetValue(name).ShouldBe(value);
            }

            return routeData;
        }

        /// <summary>
        /// Converts the URL to matching RouteData and verifies that it will match a route with the values specified by the expression.
        /// </summary>
        /// <typeparam name="TController">The type of controller</typeparam>
        /// <param name="relativeUrl">The ~/ based url</param>
        /// <param name="action">The expression that defines what action gets called (and with which parameters)</param>
        /// <returns></returns>
        public static RouteData ShouldMapTo<TController>(this string relativeUrl, Expression<Func<TController, ActionResult>> action) 
            where TController : IController
        {
            return relativeUrl.Route().ShouldMapTo(action);
        }

        /// <summary>
        /// Verifies the <see cref="RouteData">routeData</see> maps to the controller type specified.
        /// </summary>
        /// <typeparam name="TController"></typeparam>
        /// <param name="routeData"></param>
        /// <returns></returns>
        public static RouteData ShouldMapTo<TController>(this RouteData routeData) 
            where TController : IController
        {
            //strip out the word 'Controller' from the type
            string expected = typeof(TController).Name.Replace("Controller", "");

            //get the key (case insensitive)
            string actual = routeData.Values.GetValue("controller").ToString();


            expected.ShouldBe(actual);
            return routeData;
        }

        /// <summary>
        /// Verifies the <see cref="RouteData">routeData</see> will instruct the routing engine to ignore the route.
        /// </summary>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        public static RouteData ShouldBeIgnored(this string relativeUrl)
        {
            RouteData routeData = relativeUrl.Route();

            routeData.RouteHandler.ShouldBeTypeOf<StopRoutingHandler>();
            return routeData;
        }

        /// <summary>
        /// Gets a value from the <see cref="RouteValueDictionary" /> by key.  Does a
        /// case-insensitive search on the keys.
        /// </summary>
        /// <param name="routeValues"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetValue(this RouteValueDictionary routeValues, string key)
        {
            foreach (var routeValueKey in routeValues.Keys)
            {
                if (string.Equals(routeValueKey, key, StringComparison.InvariantCultureIgnoreCase))
                    return routeValues[routeValueKey] as string;
            }

            return null;
        }
    }
}
