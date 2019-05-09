using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SearchEngine.Data;

namespace SearchEngine.UI.Models
{
    public class SearchModel
    {
        public string SearchWord { get; set; }

        public List<SearchedPage> WordMatches { get; set; }
    }
}
