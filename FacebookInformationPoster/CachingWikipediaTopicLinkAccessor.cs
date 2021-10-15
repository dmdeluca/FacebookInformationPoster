using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;

namespace FacebookInformationPoster
{
    public class CachingWikipediaTopicLinkAccessor : IWikipediaTopicLinkAccessor
    {
        private readonly IWikipediaTopicLinkAccessor _decorated;
        private readonly IFile _file;

        public CachingWikipediaTopicLinkAccessor(IWikipediaTopicLinkAccessor decorated, IFile file)
        {
            _decorated = decorated;
            _file = file;
        }

        public List<string> GetHrefs(string topic)
        {
            string path = $"linkcache.{topic}";
            if (_file.Exists(path))
                return _file.ReadAllText(path).Split(" ").ToList();
            var result = _decorated.GetHrefs(topic);
            _file.WriteAllText(path, result.Aggregate((x, y) => $"{x} {y}"));
            return result;
        }
    }
}
