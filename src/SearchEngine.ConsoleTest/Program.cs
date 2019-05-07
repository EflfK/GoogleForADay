using System;
using SearchEngine.Indexer;
using System.Collections.Generic;

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
        }
    }
}
