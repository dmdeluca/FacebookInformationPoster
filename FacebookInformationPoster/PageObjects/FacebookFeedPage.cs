using OpenQA.Selenium;

namespace FacebookInformationPoster
{
    public class FacebookFeedPage : IFacebookFeedPage
    {
        private readonly IWebDriver _webDriver;

        public FacebookFeedPage(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public IWebElement StatusBox => _webDriver.FindElement(By.XPath("//*[text()[contains(.,'on your mind')]]"));
        public IWebElement StatusInput => _webDriver.FindElement(By.CssSelector("textarea.composerInput"));
        public IWebElement Submit => _webDriver.FindElement(By.CssSelector("#composer-main-view-id button"));
    }
}
