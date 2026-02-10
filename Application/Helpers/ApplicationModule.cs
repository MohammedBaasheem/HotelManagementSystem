using Application.Services;
using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Helpers
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OfferManagmentService>()
                   .AsSelf()
                   .InstancePerLifetimeScope();

            builder.RegisterType<ReservationManagmentService>()
                   .AsSelf()
                   .InstancePerLifetimeScope();

            builder.RegisterType<RoomManagmentService>()
                   .AsSelf()
                   .InstancePerLifetimeScope();

            builder.RegisterType<UserManagmentService>()
                   .AsSelf()
                   .InstancePerLifetimeScope();

            builder.RegisterType<PaymentService>()
                   .AsSelf()
                   .InstancePerLifetimeScope();
        }
    }
}
