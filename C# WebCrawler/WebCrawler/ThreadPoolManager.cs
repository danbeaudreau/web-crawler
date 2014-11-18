using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace WebCrawler
{
    class ThreadPoolManager
    {
        public static void ManageThreadPool(Crawler crawler) 
        {
            while (crawler.queueOfUrisToCrawl.Count != 0)
            {
                Uri uriToCrawl;
                bool dequeued = crawler.queueOfUrisToCrawl.TryDequeue(out uriToCrawl);
                if(dequeued)
                {
                   ThreadPool.QueueUserWorkItem(x => crawler.Crawl(uriToCrawl));
                }
            }
        }
    }
}
