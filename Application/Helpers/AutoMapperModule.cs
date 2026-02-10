using Autofac;
using AutoMapper;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Text;


namespace Application.Helpers
{
    public class AutoMapperModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // MapperConfiguration registration
            builder.Register(ctx =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddMaps(Assembly.GetExecutingAssembly()); // scan for Profiles
                });
                return config;
            })
            .AsSelf()
            .SingleInstance();

            // IMapper registration
            builder.Register(ctx =>
            {
                var config = ctx.Resolve<MapperConfiguration>();
                return config.CreateMapper(); // AutoMapper 16
            })
            .As<IMapper>()
            .InstancePerLifetimeScope();
        }
    }
}
