using System.Reflection;
using System.Web.Mvc;
using Autofac.Integration.Mvc;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using IDependencyResolver = System.Web.Mvc.IDependencyResolver;

[assembly: OwinStartup(typeof(Framework.WebUI.Startup))]

namespace Framework.WebUI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            IoC.AutoFac.Configurator.Initialize(Assembly.GetExecutingAssembly());
            var container                 = IoC.AutoFac.Configurator.Container;
            /* attach mvc resolver */
            IDependencyResolver resolver  = new AutofacDependencyResolver(container);
            var signalRResolver           = new Autofac.Integration.SignalR.AutofacDependencyResolver(container);
            DependencyResolver.SetResolver(resolver);
            GlobalHost.DependencyResolver = signalRResolver;
            var config                    = new HubConfiguration {Resolver = signalRResolver };
            app.MapSignalR("/signalr", config);
        }
    }
}
