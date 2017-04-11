using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace GoogleSearchApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            string response = GetLinks(GetResponseFromServer(@"https://www.google.com.au/search?num=1000&q=online+title+search"));
            //Console.WriteLine(response);
            Console.ReadLine();
        }

        public static string GetResponseFromServer(string url)
        {
            var request = WebRequest.Create(url);
            request.Credentials = CredentialCache.DefaultCredentials;
            using (WebResponse response = request.GetResponse())
            {
                //Console.WriteLine(((HttpWebResponse) response).StatusDescription);
                string solution = "";
                using (Stream dataStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(dataStream))
                {
                    solution =  reader.ReadToEnd();
                    return solution;
                }
            }
        }

        public static string GetLinks(string response)
        {
            const string startsWith = "<h3 class=\"r\">";
            const string endsWith = "</h3>";
            var pattern = string.Format("({0})(.*?)({1})", startsWith, endsWith);

            const string infotrack = "www.infotrack.com.au";

            var matchCollection = Regex.Matches(response, pattern);
            int i = 0;
            foreach (Match match in matchCollection)
            {
                if(match.Value.Contains(infotrack))
                    Console.WriteLine(i + " " + match.Value.Substring(0, 100) + " found at " + match.Index);
                i++;
            }
            Console.WriteLine();
            return response;
            
        }
    }
}
