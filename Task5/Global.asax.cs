using Ninject.Modules;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Task5.Models;
using Task5.BL.Util;
using Task5.Util;
using Ninject;
using Ninject.Web.Mvc;

namespace Task5
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            NinjectModule moduleBL = new ModuleBL();
            NinjectModule moduleWeb = new ModuleWeb();
            var kernel = new StandardKernel(moduleBL, moduleWeb);
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
            kernel.Unbind<ModelValidatorProvider>();
            Database.SetInitializer<ApplicationDbContext>(new AppDbInitializer());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
