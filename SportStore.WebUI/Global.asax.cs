using Ninject;
using SportStore.Domain.Entities;
using SportStore.WebUI.App_Start;
using SportStore.WebUI.Infrastructure;
using SportStore.WebUI.Infrastructure.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SportStore.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ControllerBuilder.Current.SetControllerFactory(new NinjectDependencyResolver());
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //card model binder çaðýrýlcak
            ModelBinders.Binders.Add(typeof(Cart), new CartModelBinder());
        }
    }
}
