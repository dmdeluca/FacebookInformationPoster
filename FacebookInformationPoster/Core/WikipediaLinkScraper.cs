using OpenQA.Selenium;
using Serilog;
using System.Collections.Generic;

namespace FacebookInformationPoster
{
    public class WikipediaLinkScraper : IWikipediaTopicLinkAccessor
    {
        private readonly ILogger _logger;
        private readonly IWhatLinksHerePage _whatLinksHerePage;

        public WikipediaLinkScraper(ILogger writer, IWhatLinksHerePage whatLinksHerePage)
        {
            _logger = writer;
            _whatLinksHerePage = whatLinksHerePage;
        }

        public List<string> GetHrefs(string topic)
        {
            _whatLinksHerePage.NavigateForTopicPage(topic);

            var links = new List<string> { };
            _logger.Information("scraping links");

            while (true)
            {
                foreach (var href in _whatLinksHerePage.LinkUrls)
                {
                    links.Add(href);
                    _logger.Information("scraped link " + href);
                }

                var next = _whatLinksHerePage.NextPageButton;
                if (next is null)
                    break;

                next.Click();
            }

            return links;
        }
    }
}
