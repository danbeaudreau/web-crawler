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
        private ConcurrentDictionary<string, Constants.DownloadStatus> dictionaryOfCrawledFiles;
        public ConcurrentQueue<Uri> queueOfUrisToCrawl;

        public Crawler(string startingPageString, string domainInformationString, string localDirectoryString)
        {
            //Assumes strings have already been validated prior to calling this constructor.
            startingPage = new Uri(startingPageString);
            domainInformation = new Uri(domainInformationString);
            localDirectory = new FileInfo(localDirectoryString);
            queueOfUrisToCrawl = new ConcurrentQueue<Uri>();
            dictionaryOfCrawledFiles = new ConcurrentDictionary<string, Constants.DownloadStatus>();
        }
        public bool CrawlFirstPage()
        {
            Crawl(startingPage);
            if (queueOfUrisToCrawl.Count != 0)
            {
                dictionaryOfCrawledFiles[startingPage.AbsoluteUri] = Constants.DownloadStatus.Success;
                return true;
            }
            else 
            {
                Console.WriteLine("No links were found on the starting page!");
                Console.ReadLine();
                return false;
            }
        }

        public void Crawl (object objectUri)
        {
            //this cast is required because ParameterizedThreadStart only takes a delegate with one object parameter
            Uri uri = (Uri)objectUri; 
            if (!dictionaryOfCrawledFiles.ContainsKey(uri.AbsoluteUri))
            {
                dictionaryOfCrawledFiles[uri.AbsoluteUri] = Constants.DownloadStatus.Unattempted;
            }

            if (dictionaryOfCrawledFiles[uri.AbsoluteUri] != Constants.DownloadStatus.Success)
            {
                try
                {
                    WebClient client = new WebClient();
                    client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705; WebCrawlerBeta)");
                    Stream htmlData = client.OpenRead(uri.AbsoluteUri);
                    StreamReader reader = new StreamReader(htmlData);
                    FileManager fileManager = new FileManager();
                    DataParser dataParser = new DataParser();
                    string pageHtmlAsString = reader.ReadToEnd();
                    dictionaryOfCrawledFiles[uri.AbsoluteUri] = fileManager.pipeHtmlDataToLocalFile(pageHtmlAsString, uri, localDirectory);
                    dataParser.extractLinksFromHTML(pageHtmlAsString, uri, ref queueOfUrisToCrawl);
                    htmlData.Close();
                    reader.Close();
                }
                catch (WebException webException)
                {
                    if (((HttpWebResponse)webException.Response).StatusCode == HttpStatusCode.NotFound)
                    {
                        dictionaryOfCrawledFiles[uri.AbsoluteUri] = Constants.DownloadStatus.NotFound;
                    }
                    else
                    {
                        dictionaryOfCrawledFiles[uri.AbsoluteUri] = Constants.DownloadStatus.GeneralError;
                    }
                }
            }
        }

    }
}
