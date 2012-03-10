using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class RootController : Controller
    {
        public RootController()
        {
            
        }
        //
        // GET: /Root/

        public ActionResult Index()
        {
            return View();
        }

    }
}
