using Application.IRepositories;
using Autofac;
using Infrastructure.Repositories;

namespace Infrastructure.Helpers
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // 1. Register Generic Repository
            builder.RegisterGeneric(typeof(GeneralRepository<>))
                   .As(typeof(IGeneralRepository<>))
                   .InstancePerLifetimeScope();

            builder.RegisterType<RoleRepository>()
                   .As<IRoleRepository>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<OfferManagmentRepository>()
                   .As<IOfferManagmentRepository>()
                   .InstancePerLifetimeScope();
            //IRoomManagmentRepository

            //builder.RegisterType<RoomManagmentRepository>()
            //       .As<IRoomManagmentRepository>()
            //       .InstancePerLifetimeScope();

            builder.RegisterType<UserRepository>()
                   .As<IUserRepository>()
                   .InstancePerLifetimeScope();

            // 2. Register UnitOfWork (if you use it)
            // builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
        }
    }
}
