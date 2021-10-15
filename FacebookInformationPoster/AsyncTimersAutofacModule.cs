using Autofac;

namespace FacebookInformationPoster
{
    public class AsyncTimersAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PostArticleAction>().As<IAsyncTimerAction>().InstancePerLifetimeScope();
            builder.RegisterType<PingAction>().As<IAsyncTimerAction>().InstancePerLifetimeScope();
        }
    }
}
