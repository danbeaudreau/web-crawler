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
        private string matchAllUserAgentsRegexPattern = @"(?<=User-agent:)\s+\*(.*)(?:User-agent|$)"; //this should work but it isn't in some cases[?]
        private string matchAllDisallows = @"(?:(?:Disallow:\s+(\/.*?\\n))+)";
        private URLManager urlManager;

        public DataParser()
        {
            urlManager = new URLManager();
        }

        public void ExtractLinksFromHTML(string pageHtmlAsString, Uri domainInformation, ref ConcurrentQueue<Uri> queueOfUrisToCrawl)
        {
            MatchCollection matchCollection = Regex.Matches(pageHtmlAsString, hrefRegexPattern);
            foreach (Match match in matchCollection)
            {
                if(urlManager.IsValidURL(match.Groups[1].Value))
                {
                    queueOfUrisToCrawl.Enqueue(new Uri(match.Groups[1].Value));
                }
                else if (urlManager.IsValidURL(domainInformation.AbsoluteUri.Substring(0, domainInformation.AbsoluteUri.LastIndexOf(domainInformation.AbsolutePath)) + match.Groups[1].Value)) 
                {
                    queueOfUrisToCrawl.Enqueue(new Uri(domainInformation.AbsoluteUri.Substring(0, domainInformation.AbsoluteUri.LastIndexOf(domainInformation.AbsolutePath)) + match.Groups[1].Value));
                }
                Console.WriteLine(match.Groups[1].Value);
            }
        }

        public void ExtractPathsFromRobotsDotTxt(string robotsDotTxtAsString)
        {
            Match excludedUrlsForAllBrowsers = Regex.Match(robotsDotTxtAsString, matchAllUserAgentsRegexPattern);
            MatchCollection disallowedUrls = Regex.Matches(excludedUrlsForAllBrowsers.Groups[1].Value, matchAllDisallows);

        }
    }
}
