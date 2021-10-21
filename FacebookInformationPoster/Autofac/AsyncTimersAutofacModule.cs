using Autofac;

namespace FacebookInformationPoster
{
    public class AsyncTimersAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PostArticleAction>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<PingAction>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<Scheduler>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
