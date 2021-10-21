using Autofac;
using System;

namespace FacebookInformationPoster
{
    public class PostArticleAction : IScheduledTask
    {
        private readonly ILifetimeScope _lifetimeScope;

        public PostArticleAction(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public string Name => "Post";

        public void Execute()
        {
            using var scope = _lifetimeScope.BeginLifetimeScope();
            scope.Resolve<IPostRandomWikipediaArticle>().Post();
        }
    }
}
