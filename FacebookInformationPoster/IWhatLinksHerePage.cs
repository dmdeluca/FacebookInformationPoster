using OpenQA.Selenium;
using System.Collections.Generic;

namespace FacebookInformationPoster
{
    public interface IWhatLinksHerePage
    {
        IEnumerable<string> LinkUrls { get; }
        IWebElement NextPageButton { get; }

        void NavigateForTopicPage(string topic);
    }
}