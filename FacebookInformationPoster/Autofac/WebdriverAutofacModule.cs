using Autofac;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;

namespace FacebookInformationPoster
{
    public class WebdriverAutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register((c, p) =>
            {
                var location = c.Resolve<IDictionary<string, string>>()["CHROMEDRIVER_LOCATION"];
                ChromeOptions options = new()
                {
                    UnhandledPromptBehavior = UnhandledPromptBehavior.Dismiss
                };
                //options.AddArgument("headless");
                var chromeDriver = new ChromeDriver(location, options);
                chromeDriver.Manage()
                    .Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                return chromeDriver;
            })
                .As<IWebDriver>()
                .InstancePerLifetimeScope()
                .OnRelease(x => x.Quit());
        }
    }
}
