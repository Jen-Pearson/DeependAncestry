using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Ancestry.Business;
using Ancestry.Business.Common;

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
            var file = Server.MapPath(Convert.ToBoolean(ConfigurationManager.AppSettings["UseSmallDataSample"]) ? "~/App_Data/data_small.json" : "~/App_Data/data_large.json");
            StaticCache.LoadStaticCache(file);
        }

    }
}
