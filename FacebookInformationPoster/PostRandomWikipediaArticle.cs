using System.Linq;
using System.Text;

namespace FacebookInformationPoster
{
    public class PostRandomWikipediaArticle : IPostRandomWikipediaArticle
    {
        private readonly IGetWikipediaLink _getWikipediaLink;
        private readonly IUpdateStatus _updateStatus;
        private readonly IWriter _writer;

        public PostRandomWikipediaArticle(IGetWikipediaLink getWikipediaLink, IUpdateStatus updateStatus, IWriter writer)
        {
            _getWikipediaLink = getWikipediaLink;
            _updateStatus = updateStatus;
            _writer = writer;
        }

        public void Post()
        {
            _writer.Log("started posting a wiki article.");
            var article = _getWikipediaLink.Get();
            _updateStatus.Update(new StringBuilder()
                .AppendLine($"Today's random Wikipedia article: \"{article.Title}\"")
                .AppendLine($"{article.Paragraphs.First()}")
                .AppendLine($"{article.Url}")
                .ToString());
        }
    }
}
