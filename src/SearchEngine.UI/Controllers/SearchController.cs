using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SearchEngine.UI.Models;
using SearchEngine.Data;

namespace SearchEngine.UI.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(SearchModel searchModel)
        {
            try
            {
                searchModel.WordMatches = DataAccess.ReadRankedPagesContainingWord(searchModel.SearchWord);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("SearchWord", ex.Message);
            }
            return View(searchModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
