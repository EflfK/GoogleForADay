using System;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace SearchEngine.Data
{
    public static class DataAccess
    {
        private static readonly string FILENAME = "RankedPageData.json";

        public static List<SearchedPage> ReadRankedPagesContainingWord(string word)
        {
            if (String.IsNullOrEmpty(word))
                throw new Exception("A word must be provided");

            if (word.Contains(' '))
                throw new Exception("Single words only accepted.");

            string searchWord = word.ToLower();
            List<RankedPage> rankedPages = ReadRankedPages();

            IEnumerable<SearchedPage> searchedPages = from rankedPage in rankedPages
                                                      where rankedPage.WordCounts.ContainsKey(searchWord)
                                                      orderby rankedPage.WordCounts[searchWord] descending
                                                      select new SearchedPage(rankedPage, rankedPage.WordCounts[searchWord]);

            return searchedPages.ToList();
        }

        public static void ClearRankedPages()
        {
            File.WriteAllText(FILENAME, "");
        }

        public static List<RankedPage> ReadRankedPages()
        {
            if (!File.Exists(FILENAME))
                return new List<RankedPage>();

            string json = File.ReadAllText(FILENAME);

            if (String.IsNullOrEmpty(json))
                return new List<RankedPage>();

            return JsonConvert.DeserializeObject<IEnumerable<RankedPage>>(json).ToList();
        }

        public static void AddRankedPages(IEnumerable<RankedPage> newRankedPages)
        {
            List<RankedPage> rankedPages = ReadRankedPages().ToList();

            List<RankedPage> trimmedRankedPages = rankedPages.Where(rankedPage => !newRankedPages.Contains(rankedPage)).ToList();

            WriteRankedPages(trimmedRankedPages.Concat(newRankedPages));
        }

        public static void WriteRankedPages(IEnumerable<RankedPage> rankedPages)
        {
            string json = JsonConvert.SerializeObject(rankedPages);

            File.WriteAllText(FILENAME, json);
        }
    }
}
