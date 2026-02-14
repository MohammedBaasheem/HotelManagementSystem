using Autofac;
using AutoMapper;
using Presentation.MiddleWares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Module = Autofac.Module;

namespace Presentation.Helpers
{
    public class PresentationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register Middlewares here because they belong to the Web layer
            builder.RegisterType<GlobalErrorHandlerMiddleware>().InstancePerLifetimeScope();
            builder.RegisterType<TransactionMiddleware>().InstancePerLifetimeScope();
            builder.RegisterType<RequestTimeoutMiddleware>().InstancePerLifetimeScope();

            // You can also register IHttpContextAccessor here if needed
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            // ????? ?? ??? ???? ???? ???? ??????????? ???? ?? ???????? ?????? (Presentation)

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
               .Where(t => typeof(Profile).IsAssignableFrom(t) && !t.IsAbstract && t.IsPublic)
               .As<Profile>()
               .SingleInstance();
        }
    }
}