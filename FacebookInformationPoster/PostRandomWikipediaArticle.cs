using Serilog;
using System.Linq;
using System.Text;

namespace FacebookInformationPoster
{
    public class PostRandomWikipediaArticle : IPostRandomWikipediaArticle
    {
        private readonly IGetWikipediaLink _getWikipediaLink;
        private readonly IUpdateStatus _updateStatus;
        private readonly ILogger _logger;

        public PostRandomWikipediaArticle(IGetWikipediaLink getWikipediaLink, IUpdateStatus updateStatus, ILogger logger)
        {
            _getWikipediaLink = getWikipediaLink;
            _updateStatus = updateStatus;
            _logger = logger;
        }

        public void Post()
        {
            _logger.Information("started posting a wiki article.");
            var article = _getWikipediaLink.Get();
            _updateStatus.Update(new StringBuilder()
                .AppendLine($"Today's random Wikipedia article: \"{article.Title}\"")
                .AppendLine($"{article.Paragraphs.First()}")
                .AppendLine($"{article.Url}")
                .ToString());
        }
    }
}
