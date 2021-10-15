using System.Collections.Generic;

namespace FacebookInformationPoster
{
    public interface IWikipediaTopicLinkAccessor
    {
        List<string> GetHrefs(string topic);
    }
}