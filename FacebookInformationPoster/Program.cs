using Autofac;
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
            using var scope = ConfigureContainer()
                .BeginLifetimeScope();

            foreach (var timer in scope.Resolve<IEnumerable<IAsyncTimerAction>>())
                timer.Start();

            Console.ReadKey();
        }

        private static IContainer ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(Assembly.GetEntryAssembly());
            var container = builder.Build();
            return container;
        }
    }
}
