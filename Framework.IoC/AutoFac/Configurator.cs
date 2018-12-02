using System.Data.Entity;
using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.SignalR;
using AutoMapper;
using Framework.Core.Interface.Repository;
using Framework.Core.Interface.Service;
using Framework.Core.Interface.UnitOfWork;
using Framework.Core.Model;
using Framework.Repository.Repositories;
using Framework.Repository.UnitOfWork;
using Framework.Service;

namespace Framework.IoC.AutoFac
{
    public static class Configurator
    {
        private static IContainer container;

        public static void Initialize(Assembly assembly = null)
        {
            var builder = new ContainerBuilder();
            /* entities */
            builder.Register(c => new Repository.Context.DbContext()).As<DbContext>().InstancePerLifetimeScope();

            /* automapper */
            builder.Register(c => AutoMapper.Configurator.Configure()).AsSelf().SingleInstance();
            builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>();

            /* unit of work */
            builder.Register(c => new UnitOfWorkManager(c.Resolve<DbContext>(), c.Resolve<IRepository<AuditTrail, int>>())).As<IUnitOfWorkManager>();

            /* repositories */
            builder.Register(c => new AuditTrailRepository(c.Resolve<DbContext>())).As<IRepository<AuditTrail, int>>();
            builder.Register(c => new BankRepository(c.Resolve<DbContext>())).As<IRepository<Bank, int>>();
            builder.Register(c => new BankBranchRepository(c.Resolve<DbContext>())).As<IRepository<BankBranch, int>>();

            /* services */
            builder.Register(c => new BankService(c.Resolve<IUnitOfWorkManager>(), 
                                                  c.Resolve<IRepository<Bank, int>>(),
                                                  c.Resolve<IMapper>())).As<IBankService>();

            if (assembly != null)
            {
                builder.RegisterControllers(assembly).InstancePerRequest();
                builder.RegisterHubs(assembly).ExternallyOwned();

                #region Register modules
                builder.RegisterAssemblyModules(assembly);
                #endregion

                #region Model binder providers - excluded - not sure if need
                //builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
                //builder.RegisterModelBinderProvider();
                #endregion

                #region Inject HTTP Abstractions
                /*
                 The MVC Integration includes an Autofac module that will add HTTP request 
                 lifetime scoped registrations for the HTTP abstraction classes. The 
                 following abstract classes are included: 
                -- HttpContextBase 
                -- HttpRequestBase 
                -- HttpResponseBase 
                -- HttpServerUtilityBase 
                -- HttpSessionStateBase 
                -- HttpApplicationStateBase 
                -- HttpBrowserCapabilitiesBase 
                -- HttpCachePolicyBase 
                -- VirtualPathProvider 

                To use these abstractions add the AutofacWebTypesModule to the container 
                using the standard RegisterModule method. 
                */
                builder.RegisterModule<AutofacWebTypesModule>();

                #endregion
            }
            container = builder.Build();
        }

        public static IContainer Container
        {
            get
            {
                if (container == null)
                    Initialize();
                return container;
            }
        }
    }
}
