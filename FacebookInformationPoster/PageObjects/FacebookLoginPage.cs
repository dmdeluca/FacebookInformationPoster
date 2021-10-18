using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace FacebookInformationPoster
{

    public class FacebookLoginPage : IFacebookLoginPage
    {
        private readonly IWebDriver _webDriver;
        private readonly IDictionary<string, string> _env;
        private readonly IConfiguration _configuration;

        public FacebookLoginPage(IWebDriver webDriver, IDictionary<string, string> env, IConfiguration configuration)
        {
            _webDriver = webDriver;
            _env = env;
            _configuration = configuration;
        }

        public IWebElement LoginTextBox => _webDriver.FindElement(By.Id("m_login_email"));
        public IWebElement PassTextBox => _webDriver.FindElement(By.Id("m_login_password"));
        public IWebElement SubmitButton => _webDriver.FindElement(By.Name("login"));
        public IWebElement NotNowButton => _webDriver.FindElement(By.XPath("//*[text()[contains(.,'Not Now')]]/.."));

        public void Go() => _webDriver.Navigate().GoToUrl(_configuration.GetSection("FacebookMobileUrl").Value);
    }
}
