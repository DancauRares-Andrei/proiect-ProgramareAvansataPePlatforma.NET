﻿using System.Web;
using System.Web.Mvc;

namespace proiect_ProgramareAvansataPePlatforma.NET
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
