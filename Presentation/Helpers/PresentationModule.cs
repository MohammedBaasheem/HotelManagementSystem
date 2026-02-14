using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Presentation.MiddleWares;

namespace Presentation.Helpers
{
    public class PresentationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register Middlewares here because they belong to the Web layer
            builder.RegisterType<GlobalErrorHandlerMiddleware>().InstancePerLifetimeScope();
            builder.RegisterType<TransactionMiddleware>().InstancePerLifetimeScope();
            
            // You can also register IHttpContextAccessor here if needed
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
        }
    }
}