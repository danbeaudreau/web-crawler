using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Configuration;

namespace WebCrawler
{
    class ThreadPoolManager
    {
        public static void SetMaxThreads()
        {
            string numberOfWorkerThreadsAsString = ConfigurationManager.AppSettings["WORKER_THREAD_COUNT"];
            int numberOfWorkerThreads;
            bool numberOfWorkerThreadsIsValidInt = Int32.TryParse(numberOfWorkerThreadsAsString, out numberOfWorkerThreads);

            string numberOfCompletionPortThreadsAsString = ConfigurationManager.AppSettings["COMPLETION_PORT_THREAD_COUNT"];
            int numberOfCompletionPortThreads;
            bool numberOfCompletionPortThreadsIsValidInt = Int32.TryParse(numberOfCompletionPortThreadsAsString, out numberOfCompletionPortThreads);

            if (numberOfWorkerThreadsIsValidInt && numberOfCompletionPortThreadsIsValidInt)
            {
                ThreadPool.SetMaxThreads(numberOfCompletionPortThreads, numberOfCompletionPortThreads);
            }
            //otherwise, leave the defaults
        }
        
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
