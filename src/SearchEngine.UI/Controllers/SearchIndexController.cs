using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SearchEngine.Data;
using SearchEngine.Indexer;

namespace SearchEngine.UI.Controllers
{
    public class SearchIndexController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string indexUrl, string clear, string searchIndex)
        {
            try
            {
                if (!String.IsNullOrEmpty(clear))
                {
                    DataAccess.ClearRankedPages();
                }
                else
                {
                    Crawler crawler = new Crawler(indexUrl);
                    crawler.Crawl(2).Wait();

                    IEnumerable<RankedPage> rankedPages = crawler.PagesData.Select(pd => new RankedPage
                    {
                        Title = pd.Title,
                        Url = pd.Url.AbsoluteUri,
                        WordCounts = pd.WordCounts
                    });

                    DataAccess.WriteRankedPages(rankedPages);

                    return View(new SearchEngine.UI.Models.SearchIndexModel
                    {
                        NewPageCount = rankedPages.Count(),
                        IndexUrl = indexUrl,
                        WordCount = rankedPages.SelectMany(rankedPage => rankedPage.WordCounts.Keys).Distinct().Count()
                    });
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("IndexUrl", ex.Message);
            }
            
            return View();
        }
    }
}