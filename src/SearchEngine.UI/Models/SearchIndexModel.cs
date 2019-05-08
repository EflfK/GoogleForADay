using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SearchEngine.Data;

namespace SearchEngine.UI.Models
{
    public class SearchIndexModel
    {
        public string IndexUrl { get; set; }
        public int NewPageCount { get; set; }
        public int WordCount { get; set; }
    }
}
