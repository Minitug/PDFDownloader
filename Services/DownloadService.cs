using System;
using System.Collections.Generic;
using System.Text;
using PDFDownloader.Models;

namespace PDFDownloader.Services
{
    internal class DownloadService
    {

        private readonly HttpClient http = new();

        internal async Task DownloadPdfAsync(string requestUrl, string destinationPath)
        {

            using var response = await http.GetAsync(requestUrl);

            response.EnsureSuccessStatusCode();

            using Stream source = await response.Content.ReadAsStreamAsync();

            //string /*path*/ = Path.Combine(destinationPath, "test.pdf");

            using FileStream destination = File.Create(destinationPath);

            await source.CopyToAsync(destination);
        }

        internal async Task<bool> TryDownloadItemAsync(DownloadItem item, string txtOutputFolderText)
        {
            
            string[] urls = { item.Url, item.AltUrl };
            string path = Path.Combine(txtOutputFolderText, $"{item.Id}.pdf");

            foreach (var requestUrl in urls) {

                if (string.IsNullOrEmpty(requestUrl)) continue;

                try
                {
                    await DownloadPdfAsync(requestUrl, path);

                    return true;
                }

                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    //return false;
                }
            }
            return false;
        }
    }
}
