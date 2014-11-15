using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace WebCrawler
{
    class DataParser
    {
        private string hrefRegexPattern = @"href\s*=\s*['|""]([\w+://.]*)['|""]";
        private URLManager urlManager;

        public DataParser()
        {
            urlManager = new URLManager();
        }

        public void extractLinksFromHTML(string pageHtmlAsString, ref ConcurrentQueue<Uri> crawledUris)
        {
            MatchCollection matchCollection = Regex.Matches(pageHtmlAsString, hrefRegexPattern);
            foreach (Match match in matchCollection)
            {
                //urlManager.checkUrlDomain(match.Groups[1].Value); [todo]
                Console.WriteLine(match.Groups[1].Value);
            }
        }
    }
}
