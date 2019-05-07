using System;
using System.Collections.Generic;
using System.Text;

namespace SearchEngine.Indexer
{
    public static class Helpers
    {
        public static Uri GetUri(this string url, Uri parentUrl)
        {
            Uri result;

            if (Uri.TryCreate(url, UriKind.Absolute, out result))
                return result;

            if (Uri.TryCreate(parentUrl, url, out result))
                return result;

            return null;
        }
    }
}
