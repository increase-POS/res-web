﻿using System.Web;
using System.Web.Mvc;

namespace resWebApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());

            //authorize filter
            filters.Add(new System.Web.Mvc.AuthorizeAttribute());
        }
    }
}
