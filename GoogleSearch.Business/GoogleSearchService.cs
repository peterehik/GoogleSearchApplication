using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GoogleSearch.Business
{
    public class GoogleSearchService
    {
        public string SearchUrl { get; private set; }
        public string KeyWords { get; private set; }

        private readonly string _googleUrl;

        public GoogleSearchService(string url, string keyWords)
        {
            SearchUrl = url;
            KeyWords = keyWords;
            _googleUrl = "https://www.google.com/search?num=100&q=" +
                         string.Join("+",
                             keyWords.Split(new[] {' ', ','}, StringSplitOptions.RemoveEmptyEntries)
                                 .Where(r => !string.IsNullOrWhiteSpace(r)));
        }

        public List<int> GetSearchIndexes(List<string> allResults )
        {
            const string startsWith = "<h3 class=\"r\">";
            const string endsWith = "</h3>";

            var response = GetResponseFromServer();
            var results = new List<int>();

            var reg = new Regex(string.Format("({0})(.*?)({1})", startsWith, endsWith));
            var matchCollection = reg.Matches(response);
            int i = 0;
            foreach (Match match in matchCollection)
            {
                if (match.Value.IndexOf(SearchUrl, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(i);
                    allResults.Add("<span class='makeRed'>" + match.Value + "</span>");
                }
                else
                    allResults.Add(match.Value);
                i++;
            }
            return results;
        }

        public List<int> GetSearchIndexes()
        {
            const string startsWith = "<h3 class=\"r\">";
            const string endsWith = "</h3>";

            var response = GetResponseFromServer();
            var results = new List<int>();

            var reg = new Regex(string.Format("({0})(.*?)({1})", startsWith, endsWith));
            var matchCollection = reg.Matches(response);
            int i = 0;
            foreach (Match match in matchCollection)
            {
                if (match.Value.IndexOf(SearchUrl, StringComparison.OrdinalIgnoreCase) >= 0)
                    results.Add(i);
                i++;
            }
            return results;
        }

        private string GetResponseFromServer()
        {
            var request = WebRequest.Create(_googleUrl);
            request.Credentials = CredentialCache.DefaultCredentials;
            using (var response = request.GetResponse())
            {
                using (Stream dataStream = response.GetResponseStream())
                using (var reader = new StreamReader(dataStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        
    }
}
