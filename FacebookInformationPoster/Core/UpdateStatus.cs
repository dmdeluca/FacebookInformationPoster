using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading;

namespace FacebookInformationPoster
{
    public class UpdateStatus : IUpdateStatus
    {
        private readonly IDictionary<string, string> _env;
        private readonly IFacebookLoginPage _facebookLoginPage;
        private readonly IFacebookFeedPage _facebookFeedPage;

        public UpdateStatus(
            IDictionary<string, string> env,
            IFacebookLoginPage facebookLoginPage,
            IFacebookFeedPage facebookFeedPage)
        {
            _env = env;
            _facebookLoginPage = facebookLoginPage;
            _facebookFeedPage = facebookFeedPage;
        }

        public void Update(string status)
        {
            _facebookLoginPage.Go();

            _facebookLoginPage.LoginTextBox.SendKeys(_env[Env.B64FBNUMBER]);
            _facebookLoginPage.PassTextBox.SendKeys(_env[Env.B64FBPASS]);
            _facebookLoginPage.SubmitButton.Click();
            _facebookLoginPage.NotNowButton.Click();

            _facebookFeedPage.StatusBox.Click();
            _facebookFeedPage.StatusInput.SendKeys(status);
            Thread.Sleep(2000);
            _facebookFeedPage.Submit.Click();
        }
    }
}
