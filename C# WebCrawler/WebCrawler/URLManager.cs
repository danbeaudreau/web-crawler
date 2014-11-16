using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebCrawler
{
    class URLManager
    {
        public bool isValidURL(string url)
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

        public bool verifyURLIsPartOfDomain(string urlToCheckAsString, Uri domain)
        {
            if (isValidURL(urlToCheckAsString)) 
            {
                Uri urlToCheck = new Uri(urlToCheckAsString);
                if (urlToCheck.Host == domain.Host)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else 
            {
                return false;
            }
        }

    }
}
