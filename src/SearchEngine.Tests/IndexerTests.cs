using System;
using Xunit;
using SearchEngine.Indexer;
using System.Collections.Generic;
using System.Linq;

namespace SearchEngine.Tests
{
    public class IndexerTests
    {
        [Fact]
        public async void ReadsWebpage()
        {
            string content = await Crawler.GetContent(new Uri("http://www.google.com"));

            Assert.NotNull(content);
        }
        
        [Fact]
        public void StripsHtml()
        {
            string html = "<html><head><title>some title here&laquo;</title><style>some style</style><script type=\"text/javscript\">function stuff(){}</script></head></body><body><a href=\"www.google.com\">go to google</a><b>some&nbsp; text</b></body></html>";

            string expected = "some title here go to google some text";

            string strippedHtml = Crawler.StripHtml(html);

            Assert.NotNull(strippedHtml);
            Assert.Equal(expected, strippedHtml);
        }

        [Fact]
        public void GetsChildLinks()
        {
            string html = "<html><head><title>some title here&laquo;</title></body><body><a href=\"http://www.google.com\">go to google</a><b>some&nbsp; text</b><a href=\"http://wwww.amazon.com\">go to amazon</a></body></html>";
            List<string> expected = new List<string>
            {
                "http://www.google.com/",
                "http://wwww.amazon.com/"
            };

            List<string> childLinks = Crawler.GetChildLinks(new Uri("http://www.google.com"), html).Select(url => url.AbsoluteUri).ToList();
            
            Assert.Equal(expected, childLinks);
        }

        [Fact]
        public void GetsWordCounts()
        {
            string text = "The quick brown fox jumps over the lazy dog. Fox is quick. Dog is lazy.";
            Dictionary<string, int> expected = new Dictionary<string, int>
            {
                { "the", 2 },
                { "quick", 2 },
                { "brown", 1 },
                { "fox", 2 },
                { "jumps", 1 },
                { "over", 1 },
                { "lazy", 2 },
                { "dog", 2 },
                { "is", 2 },
            };

            Dictionary<string, int> wordCounts = Crawler.GetWordCounts(text);

            Assert.Equal(expected, wordCounts);
        }

        [Fact]
        public async void GetsTitle()
        {
            string html = "<html><head><title>some title here&laquo;</title></body><body><a href=\"www.google.com\">go to google</a><b>some&nbsp; text</b></body></html>";
            string expected = "some title here&laquo;";

            string title = Crawler.GetTitle(html);
            Assert.NotNull(title);
            Assert.Equal(expected, title);
        }
    }
}
