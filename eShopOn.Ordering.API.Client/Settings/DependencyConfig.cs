using Autofac;
using eShopOn.Ordering.API.Client.Behaviors;
using eShopOn.Ordering.Behaviors;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopOn.Ordering.API.Client
{
    public class DependencyConfig : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OrderClient>().As<IOrderProvider>();
        }
    }
}