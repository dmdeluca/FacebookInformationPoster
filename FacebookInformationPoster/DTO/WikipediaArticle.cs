using System.Collections.Generic;

namespace FacebookInformationPoster
{
    public class WikipediaArticle
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Comment { get; set; }
        public List<string> Paragraphs { get; set; }
    }
}
