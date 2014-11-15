using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebCrawler
{
    class URLManager
    {
        public bool validateURL(string url)
        {
            Uri parsedURI = null;
            bool isParsedAsUri = Uri.TryCreate(url, UriKind.Absolute, out parsedURI);
            bool isHttpScheme = false;
            if (isParsedAsUri)
            {
                isHttpScheme = parsedURI.Scheme == Uri.UriSchemeHttp || parsedURI.Scheme == Uri.UriSchemeHttps;
            }
            if (isParsedAsUri && isHttpScheme)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //public bool checkUrlDomain(string url, string domain)
        //{
            
        //}

    }
}
