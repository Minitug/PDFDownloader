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

            using FileStream destination = File.Create(destinationPath);

            await source.CopyToAsync(destination);
        }

        internal async Task<DownloadResult> TryDownloadItemAsync(DownloadItem item, string txtOutputFolderText, bool skipExisting)
        {
            
            string[] urls = { item.Url, item.AltUrl };
            string path = Path.Combine(txtOutputFolderText, $"{item.Id}.pdf.tmp");
            string finalpath = Path.Combine(txtOutputFolderText, $"{item.Id}.pdf");
            string message = "Ingen gyldig PDF fundet";
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
                    await DownloadPdfAsync(requestUrl, path);

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

                catch (Exception ex)
                {
                    message = $"Kunne ikke downloade fil: {ex.Message}";
                }

                finally
                {
                    File.Delete(path);
                }
            }
            return new DownloadResult
            {
                Id = item.Id,
                Status = "Fejlet",
                Url = "",
                Message = message
            };
        }
    }
}
