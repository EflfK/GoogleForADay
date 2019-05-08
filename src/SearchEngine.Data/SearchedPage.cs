using System;
using System.Collections.Generic;
using System.Text;

namespace SearchEngine.Data
{
    public class SearchedPage : RankedPage
    {
        public SearchedPage(RankedPage rankedPage, int wordCount)
        {
            this.Title = rankedPage.Title;
            this.Url = rankedPage.Url;
            this.WordCount = wordCount;
        }

        public string Url { get; set; }

        public string Title { get; set; }

        public int WordCount { get; set; }
    }
}
