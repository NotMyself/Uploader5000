﻿using System.Web.Mvc;

namespace Web.Helpers
{
    public class JsonError : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            filterContext.Result = new JsonResult
                                       {
                                           Data = new { success = false, error = filterContext.Exception.ToString() },
                                           JsonRequestBehavior = JsonRequestBehavior.AllowGet
                                       };
        }
    }
}