using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using SearchEngine.Indexer;

namespace SearchEngine.Indexer
{
    public class Crawler
    {
        public Uri InitialUrl { get; private set; }
        public List<PageData> PagesData { get; private set; }

        public Crawler(string url)
        {
            this.InitialUrl = new UriBuilder(url).Uri;

            this.PagesData = new List<PageData>();
        }

        public async Task Crawl(int maxCrawlIndex)
        {
            IEnumerable<Uri> crawlUrls = new List<Uri> { this.InitialUrl };
            
            for(int index = 0; index < maxCrawlIndex; index++)
            {
                IEnumerable<PageData> pagesData = await Task.WhenAll(crawlUrls.Select(crawlUrl => GetPageData(crawlUrl)));
                this.PagesData.AddRange(pagesData);

                crawlUrls = GetUncrawledChildUrls();
            }
        }

        private List<Uri> GetUncrawledChildUrls()
        {
            return PagesData.SelectMany((pageData) =>
            {
                return pageData.ChildUrls;
            }).Where(childUrl => !this.PagesData.Select(pageData => pageData.Url.AbsoluteUri).ToList().Contains(childUrl.AbsoluteUri)).Distinct().ToList();
        }
        
        private async Task<PageData> GetPageData(Uri url)
        {
            string rawHtml = await GetContent(url);
            string strippedHtml = StripHtml(rawHtml);
            string title = GetTitle(rawHtml);
            List<Uri> childLinks = GetChildLinks(url, rawHtml);
            Dictionary<string, int> wordCounts = GetWordCounts(strippedHtml);
            
            return new PageData
            {
                Title = title,
                Url = url,
                WordCounts = wordCounts,
                ChildUrls = childLinks,
            };
        }

        public static string GetTitle(string rawHtml)
        {
            if (String.IsNullOrEmpty(rawHtml))
                return String.Empty;

            Match m = Regex.Match(rawHtml, @"<title>\s*(.+?)\s*</title>");

            return m.Success ? m.Groups[1].Value : "N/A";
        }

        public static string StripHtml(string rawHtml)
        {
            string strippedHtml = Regex.Replace(rawHtml, "<script((.|\n)*?)(\\/>|<\\/script>)", " ");
            strippedHtml = Regex.Replace(strippedHtml, "<style>((.|\n)*)<\\/style>", " ");
            strippedHtml = Regex.Replace(strippedHtml, "<(.|\n)*?>|&(.)*?;", " ");
            strippedHtml = Regex.Replace(strippedHtml, "[ ]{2,}", " "); // replace duplicate spaces
            return strippedHtml.Trim();
        }

        public static List<Uri> GetChildLinks(Uri parentUrl, string rawHtml)
        {
            var regex = new Regex("<(a|link).*?href=(\"|')(?<href>.+?)(\"|').*?>", RegexOptions.IgnoreCase);
            IEnumerable<string> childUrls = regex.Matches(rawHtml).Cast<Match>().Select(m => m.Groups["href"].Value);
            return childUrls.Select(childUrl => childUrl.GetUri(parentUrl)).ToList();
        }

        public static Dictionary<string, int> GetWordCounts(string text)
        {
            MatchCollection matches = Regex.Matches(text.ToLower(), @"\b[a-zA-Z']*\b"); //excludes numbers. to include numbers use: \b[\w']*\b

            return matches.Cast<Match>().Where(match => !String.IsNullOrEmpty(match.Value)).GroupBy(match => match.Value).ToDictionary(match => match.Key.ToLower(), match => match.Count());
        }

        public static async Task<string> GetContent(Uri url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url.AbsoluteUri);

            var pageContents = await response.Content.ReadAsStringAsync();

            return pageContents;
        }
    }
}
