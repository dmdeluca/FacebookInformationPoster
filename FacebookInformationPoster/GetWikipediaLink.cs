using OpenQA.Selenium;
using System;
using System.IO.Abstractions;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace FacebookInformationPoster
{
    public class GetWikipediaLink : IGetWikipediaLink
    {
        private readonly IWebDriver _webDriver;
        private readonly IFile _file;
        private readonly IWikipediaTopicLinkAccessor _wikipediaTopicLinkAccessor;
        private static readonly Regex _referenceRegex = new(@"\[\d+\]");

        public GetWikipediaLink(IWebDriver webDriver, IFile file, IWikipediaTopicLinkAccessor wikipediaTopicLinkAccessor)
        {
            _webDriver = webDriver;
            _file = file;
            _wikipediaTopicLinkAccessor = wikipediaTopicLinkAccessor;
        }

        public WikipediaArticle Get()
        {
            var links = _file.ReadAllText("topics")
                .ToLower()
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                .SelectMany(x => _wikipediaTopicLinkAccessor.GetHrefs(x))
                .ToList();

            var random = new Random();
            var link = links.ElementAt(random.Next(links.Count));
            _webDriver.Navigate().GoToUrl(link);

            var url = _webDriver.Url;
            return new WikipediaArticle
            {
                Url = url,
                Comment = "No comment",
                Title = _webDriver.Title.Split(" - ").First(),
                Paragraphs = _webDriver.FindElements(By.CssSelector("#mw-content-text p")).Take(3)
                    .Select(x => x.Text)
                    .Select(x => _referenceRegex.Replace(x, string.Empty))
                    .ToList()
            };
        }

        private string[] _topics => _file.ReadAllText("topics")
            .ToLower()
            .Split(Environment.NewLine);
    }
}
