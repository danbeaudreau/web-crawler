using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Net;
using System.Collections.Concurrent;

namespace WebCrawler
{
    class Crawler
    {
        private Uri startingPage;
        private Uri domainInformation;
        private FileInfo localDirectory;
        private SemaphoreSlim semaphore;
        private DataParser dataParser;
        private ConcurrentQueue<Uri> crawledUris;
        private const int maxCount = 10000; 

        public Crawler(string startingPageString, string domainInformationString, string localDirectoryString)
        {
            //Assumes strings have already been validated prior to calling this constructor.
            startingPage = new Uri(startingPageString);
            domainInformation = new Uri(domainInformationString);
            localDirectory = new FileInfo(localDirectoryString);
            semaphore = new SemaphoreSlim(0, maxCount);
            dataParser = new DataParser();
            crawledUris = new ConcurrentQueue<Uri>();
        }
        public void CrawlFirstPage()
        {
            WebClient client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            Stream htmlData = client.OpenRead(startingPage.AbsoluteUri);
            StreamReader reader = new StreamReader(htmlData);
            string pageHtmlAsString = reader.ReadToEnd();
            dataParser.extractLinksFromHTML(pageHtmlAsString, ref crawledUris);
            htmlData.Close();
            reader.Close();
        }
    }
}
