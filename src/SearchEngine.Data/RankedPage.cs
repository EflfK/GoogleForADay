using System;
using System.Collections.Generic;
using System.Text;

namespace SearchEngine.Data
{
    public class RankedPage : IEquatable<RankedPage>
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public Dictionary<string, int> WordCounts { get; set; }

        public bool Equals(RankedPage other)
        {
            return this.Title == other.Title;
        }
    }
}
