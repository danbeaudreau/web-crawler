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
        private WebClient client;
        private List<Uri> listOfCrawledFiles;
        private const int maxCount = 10000; 

        public Crawler(string startingPageString, string domainInformationString, string localDirectoryString)
        {
            //Assumes strings have already been validated prior to calling this constructor.
            startingPage = new Uri(startingPageString);
            domainInformation = new Uri(domainInformationString);
            localDirectory = new FileInfo(localDirectoryString);
            dataParser = new DataParser();
            crawledUris = new ConcurrentQueue<Uri>();
            listOfCrawledFiles = new List<Uri>();
            client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
        }
        public void CrawlFirstPage()
        {
            Stream htmlData = client.OpenRead(startingPage.AbsoluteUri);
            StreamReader reader = new StreamReader(htmlData);
            FileManager fm = new FileManager();
            string pageHtmlAsString = reader.ReadToEnd();
            fm.pipeHtmlDataToLocalFile(pageHtmlAsString, startingPage, localDirectory);
            dataParser.extractLinksFromHTML(pageHtmlAsString, domainInformation, ref crawledUris);
            htmlData.Close();
            reader.Close();
            if (crawledUris.Count != 0)
            {
                listOfCrawledFiles.Add(startingPage);
                ManageConcurrentCrawling();
            }
            else 
            {
                Console.WriteLine("No links were found on the starting page!");
                Console.ReadLine();
            }
        }

        public void Crawl (Uri uri)
        {
            if(!listOfCrawledFiles.Contains(uri))
            {
                Stream htmlData = client.OpenRead(uri.AbsoluteUri);
                StreamReader reader = new StreamReader(htmlData);
                FileManager fm = new FileManager();
                string pageHtmlAsString = reader.ReadToEnd();
                fm.pipeHtmlDataToLocalFile(pageHtmlAsString, uri, localDirectory);
                dataParser.extractLinksFromHTML(pageHtmlAsString, uri, ref crawledUris);
                htmlData.Close();
                reader.Close();
            }
        }

        public void ManageConcurrentCrawling()
        {
            semaphore = new SemaphoreSlim(0, maxCount);
            //to-do implement concurrent crawling

        }
    }
}
