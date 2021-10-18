using OpenQA.Selenium;
using System.Collections.Generic;

namespace FacebookInformationPoster
{
    public class FacebookLoginPage : IFacebookLoginPage
    {
        private readonly IWebDriver _webDriver;
        private readonly IDictionary<string, string> _env;

        public FacebookLoginPage(IWebDriver webDriver, IDictionary<string, string> env)
        {
            _webDriver = webDriver;
            _env = env;
        }

        public IWebElement LoginTextBox => _webDriver.FindElement(By.Id("m_login_email"));
        public IWebElement PassTextBox => _webDriver.FindElement(By.Id("m_login_password"));
        public IWebElement SubmitButton => _webDriver.FindElement(By.Name("login"));

        public void Go() => _webDriver.Navigate().GoToUrl("https://m.facebook.com");
    }
}
