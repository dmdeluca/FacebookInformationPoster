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
        private readonly IFacebookLoginPage _facebookLoginPage;

        public UpdateStatus(IWebDriver webDriver, IDictionary<string, string> env, IFacebookLoginPage facebookLoginPage)
        {
            _webDriver = webDriver;
            _env = env;
            _facebookLoginPage = facebookLoginPage;
        }

        public void Update(string status)
        {
            _facebookLoginPage.Go(); 
            _facebookLoginPage.LoginTextBox.SendKeys(_env[Env.B64FBNUMBER]);
            _facebookLoginPage.PassTextBox.SendKeys(_env[Env.B64FBPASS]);
            _facebookLoginPage.SubmitButton.Click();

            _webDriver.FindElement(By.XPath("//*[text()[contains(.,'Not Now')]]/..")).Click();
            _webDriver.FindElement(By.XPath("//*[text()[contains(.,'on your mind')]]")).Click();
            _webDriver.FindElement(By.CssSelector("textarea.composerInput")).SendKeys(status);
            Thread.Sleep(2000);
            _webDriver.FindElement(By.CssSelector("#composer-main-view-id button")).Click();
        }
    }
}
