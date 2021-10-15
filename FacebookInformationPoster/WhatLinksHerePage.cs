using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;

namespace FacebookInformationPoster
{
    public class WhatLinksHerePage : IWhatLinksHerePage
    {
        private readonly IWebDriver _webDriver;
        public WhatLinksHerePage(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public void NavigateForTopicPage(string topic)
            => _webDriver.Navigate().GoToUrl($"https://en.wikipedia.org/w/index.php?title=Special:WhatLinksHere/{topic}&namespace=0&limit=5000&hideredirs=1&hidetrans=1");

        public IEnumerable<string> LinkUrls
            => _webDriver.FindElements(By.CssSelector("#mw-whatlinkshere-list li > a")).Select(x => x.GetAttribute("href"));

        public IWebElement NextPageButton
            => _webDriver.FindElements(By.CssSelector("a[href*='dir=next']")).FirstOrDefault(x => !x.GetAttribute("href").Contains("https://en.m.wikipedia.org/"));
    }
}
