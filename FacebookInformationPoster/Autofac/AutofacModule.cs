using Autofac;
using Microsoft.Extensions.Configuration;
using RestSharp;
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

            var configurationFromJson = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            builder.RegisterInstance(configurationFromJson)
                .As<IConfiguration>()
                .SingleInstance();

            builder.Register((c, p) =>
            {
                return new RestClient();
            })
                .As<IRestClient>()
                .InstancePerDependency();

            builder.RegisterInstance(configuration).As<IDictionary<string, string>>();
            builder.RegisterType<UpdateStatus>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<GetWikipediaLink>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<PostRandomWikipediaArticle>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<FileSystem>().As<IFileSystem>().SingleInstance();
            builder.RegisterType<FileWrapper>().As<IFile>().SingleInstance();
            builder.RegisterType<DirectoryWrapper>().As<IDirectory>().SingleInstance();
            builder.RegisterType<PathWrapper>().As<IPath>().SingleInstance();
            builder.RegisterType<WikipediaApiLinkRetriever>().AsImplementedInterfaces().InstancePerLifetimeScope();
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
