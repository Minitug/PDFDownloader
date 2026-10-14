using System;
using System.Collections.Generic;
using System.Text;

namespace PDFDownloader.Models
{
    internal class DownloadResult
    {
        internal string Id { get; set; } = "";
        internal string Status { get; set; } = "";
        internal string Url { get; set; } = "";
        internal string Message { get; set; } = "";
    }
}
