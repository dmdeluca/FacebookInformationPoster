using OpenQA.Selenium;

namespace FacebookInformationPoster
{
    public interface IFacebookLoginPage
    {
        IWebElement LoginTextBox { get; }
        IWebElement PassTextBox { get; }
        IWebElement SubmitButton { get; }

        void Go();
    }
}