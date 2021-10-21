using Autofac;
using Autofac.Extras.Moq;
using Moq;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FacebookInformationPoster.Tests
{
    public class PostRandomWikipediaArticleTests
    {
        [Fact]
        public void PostRandomWikipediaArticleTests_HappyPath()
        {
            // arrange
            using var am = AutoMock.GetLoose();
            var article = new WikipediaArticle
            {
                Title = "Things",
                Url = "https://en.wikipedia.org/Things",
                Paragraphs = new List<string>
                {
                    "Lorem",
                    "ipsum",
                }
            };
            am.Mock<IGetWikipediaLink>()
                .Setup(x => x.Get())
                .Returns(article)
                .Verifiable();

            // act
            am.Create<PostRandomWikipediaArticle>().Post();

            // assert
            am.Mock<IUpdateStatus>()
                .Verify(x => x.Update(It.Is<string>(y => y.Contains("Lorem") && y.Contains(article.Title) && y.Contains(article.Url))), Times.Once);
        }
    }

    public class RestRequestToStringTests
    {
        [Fact]
        public void RestRequestToStringTest()
        {
            var request = new RestRequest()
                .AddQueryParameter("prop", "linkshere")
                .AddQueryParameter("action", "query")
                .AddQueryParameter("format", "json")
                .AddQueryParameter("lhshow", "!redirect")
                .AddQueryParameter("lhlimit", "max")
                .AddQueryParameter("titles", "philosophy");

            var stringified = "?" + request.Parameters.Select(x => $"{x.Name}={x.Value}").Aggregate((x, y) => $"{x}&{y}");
        }
    }
}
