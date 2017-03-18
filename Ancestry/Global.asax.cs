using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Ancestry.Business;

namespace Ancestry
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            LoadData();
        }

        private void LoadData()
        {
            var service = new DataService();
            var file = Server.MapPath("~/App_Data/data_small.json");
            HttpContext.Current.Application["data"] = service.ReadFile(file);
        }

    }
}
