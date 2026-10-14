using System;
using System.Collections.Generic;
using System.Text;

namespace PDFDownloader.Models
{
    internal class DownloadItem
    {
        public string Id { get; set; } = "";
        public string Url { get; set; } = "";
        public string AltUrl { get; set; } = "";
    }
}
