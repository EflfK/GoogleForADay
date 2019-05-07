using System;
using System.Collections.Generic;
using System.Text;

namespace SearchEngine.Indexer
{
    public class PageData
    {
        public string Title { get; set; }
        public Uri Url { get; set; }
        public Dictionary<string, int> WordCounts { get; set; }
        public List<Uri> ChildUrls { get; set; }
    }
}
