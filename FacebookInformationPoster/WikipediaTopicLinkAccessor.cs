using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;

namespace FacebookInformationPoster
{
    public class WikipediaTopicLinkAccessor : IWikipediaTopicLinkAccessor
    {
        private readonly IWebDriver _webDriver;
        private readonly Writer _writer;

        public WikipediaTopicLinkAccessor(IWebDriver webDriver, Writer writer)
        {
            _webDriver = webDriver;
            _writer = writer;
        }

        public List<string> GetHrefs(string topic)
        {
            _webDriver.Navigate().GoToUrl($"https://en.wikipedia.org/w/index.php?title=Special:WhatLinksHere/{topic}&namespace=0&limit=5000&hideredirs=1&hidetrans=1");

            var links = new List<string> { };
            while (true)
            {
                foreach (var href in _webDriver.FindElements(By.CssSelector("#mw-whatlinkshere-list li > a")).Select(x => x.GetAttribute("href")))
                {
                    links.Add(href);
                    _writer.Log(href);
                }

                var next = _webDriver.FindElements(By.CssSelector("a[href*='dir=next']")).FirstOrDefault(x => !x.GetAttribute("href").Contains("https://en.m.wikipedia.org/"));
                if (next is null)
                    break;
                next.Click();
            }

            return links;
        }
    }
}
