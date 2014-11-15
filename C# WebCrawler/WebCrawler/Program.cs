using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebCrawler
{
    class Program
    {

        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Error, the program was passed an incorrect amount of parameters.");
                Console.ReadLine();
            }

            string startingPage = args[0];
            string domainInformation = args[1];
            string localDirectory = args[2];
            

            URLManager urlManager = new URLManager();
            FileManager fileManager = new FileManager();

            if(!urlManager.validateURL(startingPage))
            {
                Console.WriteLine("Error, the starting page URL is not a valid HTTP or HTTPS URL.");
                Console.ReadLine();

            }
            if(!urlManager.validateURL(domainInformation))
            {
                Console.WriteLine("Error, the domain URL is not a valid HTTP or HTTPS URL.");
                Console.ReadLine();
            }
            if (!fileManager.validateFilePath(localDirectory))
            {
                Console.WriteLine("Error, the local directory is not valid or the application does not have write access.");
                Console.ReadLine();
            }

            Crawler webCrawler = new Crawler(startingPage, domainInformation, localDirectory);
            //webCrawler.Crawl();

            Console.WriteLine("Web Crawler terminated.");
            Console.ReadLine();

        }

    }
}
