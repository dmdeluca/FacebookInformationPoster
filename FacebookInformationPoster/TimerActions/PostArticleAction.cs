using System;

namespace FacebookInformationPoster
{
    public class PostArticleAction : AsyncTimerAction
    {
        private readonly IPostRandomWikipediaArticle _postRandomWikipediaArticle;

        public PostArticleAction(IPostRandomWikipediaArticle postRandomWikipediaArticle)
        {
            _postRandomWikipediaArticle = postRandomWikipediaArticle;
        }

        public override void Action()
        {
            _postRandomWikipediaArticle.Post();
        }

        public override void OnConfiguring(AsyncTimerActionOptions options)
        {
            options.ActionInterval = TimeSpan.FromHours(3);
            options.Limit = 1;
            options.LimitSpan = TimeSpan.FromDays(1);
            options.StartOffset = new TimeSpan(hours: 7 + 12, minutes: 11, seconds: 0);
        }
    }
}
