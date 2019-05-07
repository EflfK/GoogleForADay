using System;
using Xunit;
using SearchEngine.Indexer;

namespace SearchEngine.Tests
{
    public class IndexerTests
    {
        [Fact]
        public async void ReadsWebpage()
        {
            string content = await Crawler.GetContent("http://www.google.com");

            Assert.NotNull(content);
        }
        
        [Fact]
        public async void StripsHtml()
        {
            string html = "<html><head><title>some title here&laquo;</title></body><body><a href=\"www.google.com\">go to google</a><b>some&nbsp; text</b></body></html>";
            string expected = "some title here go to google some text";

            string strippedHtml = Crawler.StripHtml(html);

            Assert.NotNull(strippedHtml);
            Assert.Equal(expected, strippedHtml);
        }

        [Fact]
        public async void GetsTitle()
        {
            Crawler crawler = new Crawler("https://www.youtube.com/");
            await crawler.Crawl();

            Assert.NotNull(crawler.Title);
            Assert.Equal("YouTube", crawler.Title);
        }
    }
}
