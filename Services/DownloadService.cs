using PDFDownloader.Models;
using System.Diagnostics;

namespace PDFDownloader.Services
{
    internal class DownloadService
    {

        private readonly HttpClient http = new()
        {
            //Timeout = TimeSpan.FromMinutes(4)
        };

        internal async Task DownloadPdfAsync(
            string requestUrl, 
            string destinationPath, 
            CancellationToken cancellationToken
        )
        {
            //http.Timeout = TimeSpan.FromMinutes(4);

            using var response = await http.GetAsync(requestUrl, cancellationToken);

            response.EnsureSuccessStatusCode();

            using Stream source = await response.Content.ReadAsStreamAsync();

            using FileStream destination = File.Create(destinationPath);

            await source.CopyToAsync(destination, cancellationToken);
        }

        internal async Task<DownloadResult> TryDownloadItemAsync(
            DownloadItem item, 
            string txtOutputFolderText, 
            bool skipExisting, 
            CancellationToken cancellationToken
        )
        {
            
            string[] urls = { item.Url, item.AltUrl };
            string path = Path.Combine(txtOutputFolderText, $"{item.Id}.pdf.tmp");
            string finalpath = Path.Combine(txtOutputFolderText, $"{item.Id}.pdf");
            string message = "Ingen gyldig PDF fundet";
            List<string> errors = new();
            var pdfValidator = new PdfValidator();
            bool isPdf;

            if (skipExisting && File.Exists(finalpath))
            {
                if (pdfValidator.IsPdf(finalpath))
                {
                    return new DownloadResult
                    {
                        Id = item.Id,
                        Status = "Sprunget over",
                        Url = "",
                        Message = "Der er allerede en fil med dette BR-Nummer"
                    };
                }
            }

            if (string.IsNullOrWhiteSpace(item.Url) &&
                string.IsNullOrWhiteSpace(item.AltUrl))
            {
                return new DownloadResult
                {
                    Id = item.Id,
                    Status = "Fejlet",
                    Url = "",
                    Message = "Ingen URL angivet"
                };
            }

            foreach (var requestUrl in urls) {

                if (string.IsNullOrWhiteSpace(requestUrl)) continue;

                try
                {
                    await DownloadPdfAsync(requestUrl, path, cancellationToken);

                    isPdf = pdfValidator.IsPdf(path);

                    if (isPdf)
                    {
                        File.Move(path, finalpath, overwrite: true);

                        message = requestUrl == item.Url
                        ? "Downloadet fra primær URL"
                        : "Downloadet fra alternativ URL";

                        return new DownloadResult
                        {
                            Id = item.Id,
                            Status = "Downloadet",
                            Url = requestUrl,
                            Message = message
                        };
                    }
                    else 
                    {
                        message = "Fandt fil, men kunne ikke validere PDF format";       
                    }

                    
                }
                catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
                {
                    message = "HTTP-anmodningen fik timeout.";
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    message = $"Kunne ikke downloade fil: {ex.Message}";
                }

                finally
                {
                    File.Delete(path);
                }

                string urlType = requestUrl == item.Url ? "Primær" : "Alternativ";
                errors.Add($"{urlType}: {message}");
            }
            return new DownloadResult
            {
                Id = item.Id,
                Status = "Fejlet",
                Url = "",
                Message = string.Join(" | ", errors)
            };
        }
    }
}
