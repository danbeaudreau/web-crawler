using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WebCrawler
{
    class Crawler
    {
        private Uri startingPage;
        private Uri domainInformation;
        private FileInfo localDirectory;
        public Crawler(string startingPageString, string domainInformationString, string localDirectoryString)
        {
            startingPage = new Uri(startingPageString);
            domainInformation = new Uri(domainInformationString);
            localDirectory = new FileInfo(localDirectoryString);
        }
    }
}
