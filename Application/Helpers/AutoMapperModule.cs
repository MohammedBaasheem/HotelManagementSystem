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
        private readonly IEnumerable<Assembly> _additionalAssemblies;

        // 2. Constructor ياخد الـ Assemblies دي (اختياري)
        public AutoMapperModule(IEnumerable<Assembly> additionalAssemblies = null)
        {
            _additionalAssemblies = additionalAssemblies ?? Enumerable.Empty<Assembly>();
        }
        protected override void Load(ContainerBuilder builder)
        {

            // MapperConfiguration registration
            builder.Register(ctx =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddMaps(Assembly.GetExecutingAssembly()); // scan for Profiles

                    foreach (var assembly in _additionalAssemblies)
                    {
                        cfg.AddMaps(assembly);
                    }
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
