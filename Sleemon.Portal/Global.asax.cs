using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Sleemon.Portal.Common;
using Sleemon.Portal.Controllers;
using Sleemon.Portal.Core;
using Sleemon.Portal.Factories;
using System;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Sleemon.Portal
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private UnityContainer container;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;

            this.ConfigureIocContainer(GlobalConfiguration.Configuration);
        }
        
        protected void Application_Error(object sender, EventArgs e)
        {
            var httpContext = new HttpContextWrapper(((MvcApplication)sender).Context);
            var ex = Server.GetLastError();

            httpContext.ClearError();
            httpContext.Response.Clear();
            httpContext.Response.ContentType = "text/html";
            httpContext.Response.StatusCode = 200;
            httpContext.Response.TrySkipIisCustomErrors = true;

            var httpException = ex as HttpException ?? new HttpException(500, "Internal Server Error");

            // TODO: Log Error
            var routeData = new RouteData();
            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = "Index";
            routeData.Values["errorCode"] = httpException.GetHttpCode();

            ((IController)new ErrorController()).Execute(new RequestContext(httpContext, routeData));
        }

        private void ConfigureIocContainer(HttpConfiguration config)
        {
            this.container = new UnityContainer();

            this.container.RegisterInstance(new ServiceFactory(this.container), new ContainerControlledLifetimeManager());
            
            var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            this.container.LoadConfiguration(section, "UnityContainer");
            this.container.RegisterInstance(this.container.Resolve<ImplementServiceClient>(), new ContainerControlledLifetimeManager());

            ControllerBuilder.Current.SetControllerFactory(new UnityControllerFactory(this.container));

            config.DependencyResolver = new UnityDependencyResolver(this.container);

            var filterProvider = FilterProviders.Providers.Single(p => p is FilterAttributeFilterProvider);
            FilterProviders.Providers.Remove(filterProvider);

            var unityFilterAttributeFilterProvider = new UnityFilterAttributeFilterProvider(this.container);
            FilterProviders.Providers.Add(unityFilterAttributeFilterProvider);
        }
    }
}
