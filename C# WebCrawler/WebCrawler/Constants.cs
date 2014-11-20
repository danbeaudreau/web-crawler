using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebCrawler
{
    class Constants
    {
        public enum DownloadStatus
        {
            Success,
            Unattempted,
            FileWriteError,
            Timeout,
            NotFound,
            GeneralError
        };

        public enum RobotsTxtStatus
        {
            Success,
            ParseError,
            NotFound,
            Unattempted,
            GeneralError
        }
    }
}
