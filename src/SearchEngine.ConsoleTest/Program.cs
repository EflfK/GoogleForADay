using System;
using SearchEngine.Indexer;
using System.Collections.Generic;
using SearchEngine.Data;
using System.Linq;

namespace SearchEngine.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://www.google.com";

            Crawler crawler = new Crawler(url);
            crawler.Crawl(2).Wait();
            List<PageData> pagesData = crawler.PagesData;

            IEnumerable<RankedPage> rankedPages = pagesData.Select(pd => new RankedPage
            {
                Title = pd.Title,
                Url = pd.Url.AbsoluteUri,
                WordCounts = pd.WordCounts
            });

            DataAccess.WriteRankedPages(rankedPages);

            var searchedPages = DataAccess.ReadRankedPagesContainingWord("google");
        }
    }
}
