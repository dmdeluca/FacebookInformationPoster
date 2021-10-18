using OpenQA.Selenium;

namespace FacebookInformationPoster
{
    public interface IFacebookFeedPage
    {
        IWebElement StatusBox { get; }
        IWebElement StatusInput { get; }
        IWebElement Submit { get; }
    }
}