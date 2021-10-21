using Autofac;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;

namespace FacebookInformationPoster
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            logger.Information("application has started.");

            var builder = new ContainerBuilder();
            builder.RegisterInstance(logger).AsImplementedInterfaces();
            builder.RegisterAssemblyModules(Assembly.GetEntryAssembly());
            var container = builder.Build();

            using var scope = container.BeginLifetimeScope();

            logger.Information("starting timers.");
            scope.Resolve<IScheduler>().StartPolling();

            Console.ReadKey();
        }
    }
}
