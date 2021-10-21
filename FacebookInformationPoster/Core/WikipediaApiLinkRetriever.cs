using Microsoft.Extensions.Configuration;
using RestSharp;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace FacebookInformationPoster
{
    public class WikipediaApiLinkRetriever : IWikipediaTopicLinkAccessor
    {
        private readonly IRestClient _restClient;
        private readonly ILogger _logger;

        public WikipediaApiLinkRetriever(IRestClient restClient, ILogger logger, IConfiguration configuration)
        {
            _restClient = restClient;
            _logger = logger;
            _restClient.BaseUrl = new System.Uri(configuration.GetSection("WikipediaApiBaseUrl").Value);
        }

        public List<string> GetHrefs(string topic)
        {
            var links = new HashSet<string>();

            string lhcontinue = string.Empty;
            string @continue = string.Empty;

            while (true)
            {
                var request = new RestRequest()
                  .AddQueryParameter("prop", "linkshere")
                  .AddQueryParameter("action", "query")
                  .AddQueryParameter("format", "json")
                  .AddQueryParameter("lhshow", "!redirect")
                  .AddQueryParameter("lhlimit", "max")
                  .AddQueryParameter("titles", "philosophy");

                if (!string.IsNullOrEmpty(lhcontinue) && !string.IsNullOrEmpty(@continue))
                    request.AddQueryParameter("lhcontinue", lhcontinue)
                        .AddQueryParameter("continue", @continue);

                var stringified = "?" + request.Parameters.Select(x => $"{x.Name}={x.Value}").Aggregate((x, y) => $"{x}&{y}");
                _logger.Information($"sending request: {_restClient.BaseUrl}{stringified}");

                var response = _restClient.Get(request);
                var json = JsonDocument.Parse(response.Content);

                var titles = json.RootElement
                    .GetProperty("query")
                    .GetProperty("pages")
                    .EnumerateObject().SelectMany(x =>
                    {
                        return x.Value.GetProperty("linkshere")
                            .EnumerateArray()
                            .Select(x => x.GetProperty("title").GetString())
                            .Where(x => !x.StartsWith("Talk"))
                            .Where(x => !x.StartsWith("User"));
                    })
                    .Select(x=>$"https://en.wikipedia.org/wiki/{x}")
                    .Distinct();

                foreach (var title in titles)
                    links.Add(title);

                if (!json.RootElement.TryGetProperty("continue", out var @continueElement) || !continueElement.TryGetProperty("lhcontinue", out var continueChild))
                    break;

                lhcontinue = continueChild.GetString();

                @continue = json.RootElement
                    .GetProperty("continue")
                    .GetProperty("continue")
                    .GetString();
            }

            return links.ToList();
        }
    }
}
