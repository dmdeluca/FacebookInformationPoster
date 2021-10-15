using Autofac;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FacebookInformationPoster
{
    public class AutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            var configuration = typeof(Env).GetMembers(BindingFlags.Public | BindingFlags.Static)
                .ToDictionary(x => x.Name, x => RegisterEnvironmentVariable(x));

            builder.RegisterInstance(configuration).As<IDictionary<string, string>>();
            builder.RegisterType<UpdateStatus>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<GetWikipediaLink>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<PostRandomWikipediaArticle>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<FileSystem>().As<IFileSystem>().SingleInstance();
            builder.RegisterType<FileWrapper>().As<IFile>().SingleInstance();
            builder.RegisterType<DirectoryWrapper>().As<IDirectory>().SingleInstance();
            builder.RegisterType<PathWrapper>().As<IPath>().SingleInstance();
            builder.RegisterType<WikipediaLinkScraper>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterDecorator<CachingWikipediaTopicLinkAccessor, IWikipediaTopicLinkAccessor>();
        }

        private static string RegisterEnvironmentVariable(MemberInfo x)
        {
            var value = Environment.GetEnvironmentVariable(x.Name);
            if (value is null)
                throw new Exception($"Environment variable '{x.Name}' cannot be null.");

            if (x.Name.StartsWith("B64"))
                return value.Decode();

            return value;
        }
    }
}
