using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading;

namespace FacebookInformationPoster
{
    public class UpdateStatus : IUpdateStatus
    {
        private readonly IWebDriver _webDriver;
        private readonly IDictionary<string, string> _env;

        public UpdateStatus(IWebDriver webDriver, IDictionary<string, string> env)
        {
            _webDriver = webDriver;
            _env = env;
        }

        public void Update(string status)
        {
            _webDriver.Navigate().GoToUrl("https://m.facebook.com");
            _webDriver.FindElement(By.Id("m_login_email")).SendKeys(_env[Env.B64FBNUMBER]);
            _webDriver.FindElement(By.Id("m_login_password")).SendKeys(_env[Env.B64FBPASS]);
            _webDriver.FindElement(By.Name("login")).Click();
            _webDriver.FindElement(By.XPath("//*[text()[contains(.,'Not Now')]]/..")).Click();
            _webDriver.FindElement(By.XPath("//*[text()[contains(.,'on your mind')]]")).Click();
            _webDriver.FindElement(By.CssSelector("textarea.composerInput")).SendKeys(status);
            Thread.Sleep(2000);
            _webDriver.FindElement(By.CssSelector("#composer-main-view-id button")).Click();
        }
    }
}
