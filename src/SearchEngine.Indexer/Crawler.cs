using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SearchEngine.Indexer
{
    public class Crawler
    {
        public string RawHtml { get; private set;}
        public string StrippedHtml { get; private set; }
        public string Url { get; private set; }
        public string Title { get; private set; }

        public Crawler(string url)
        {
            this.Url = url;
        }

        public async Task Crawl()
        {
            this.RawHtml = await GetContent(this.Url);
            this.StrippedHtml = StripHtml(this.RawHtml);
            this.Title = GetTitle(this.RawHtml);
        }

        public static string GetTitle(string rawHtml)
        {
            if (String.IsNullOrEmpty(rawHtml))
                return String.Empty;

            Match m = Regex.Match(rawHtml, @"<title>\s*(.+?)\s*</title>");

            return m.Success ? m.Groups[1].Value : String.Empty;
        }

        public static string StripHtml(string rawHtml)
        {
            string strippedHtml = Regex.Replace(rawHtml, "<(.|\n)*?>|&(.)*?;", " ");
            strippedHtml = Regex.Replace(strippedHtml, "[ ]{2,}", " "); // replace duplicate spaces
            return strippedHtml.Trim();
        }

        public static async Task<string> GetContent(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);

            var pageContents = await response.Content.ReadAsStringAsync();

            return pageContents;
        }
    }
}
