using Autofac;

namespace FacebookInformationPoster
{
    public class PageObjectsAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WhatLinksHerePage>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
                
            builder.RegisterType<FacebookLoginPage>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
