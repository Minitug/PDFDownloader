using PDFDownloader.Models;
using PDFDownloader.Services;

namespace PDFDownloader.Tests
{
    public class DownloadServiceTests
    {
        [Fact]
        public async Task TryDownloadItemAsync_NoUrls_ReturnsFailed()
        {
            // Arrange
            var service = new DownloadService();

            var item = new DownloadItem
            {
                Id = "123",
                Url = "",
                AltUrl = ""
            };

            string folder = Path.GetTempPath();

            // Act
            DownloadResult result = await service.TryDownloadItemAsync(
                item,
                folder,
                skipExisting: false,
                CancellationToken.None
            );

            // Assert
            Assert.Equal("Fejlet", result.Status);
            Assert.Equal("Ingen URL angivet", result.Message);
        }

        [Fact]
        public async Task TestSkippingFunction_ReturnSkip()
        {
            var service = new DownloadService();

            string folder = Path.Combine(
                Path.GetTempPath(),
                Guid.NewGuid().ToString()
            );

            try
            {
                Directory.CreateDirectory(folder);

                string pdfPath = Path.Combine(folder, "123.pdf");

                await File.WriteAllTextAsync(pdfPath, "%PDF-1.7\nTest content");

                var item = new DownloadItem
                {
                    Id = "123",
                    Url = "https://example.com/document.pdf",
                    AltUrl = ""
                };

                DownloadResult result = await service.TryDownloadItemAsync(
                    item,
                    folder,
                    skipExisting: true,
                    CancellationToken.None
                );

                Assert.Equal("Sprunget over", result.Status);

                Assert.Equal(
                    "%PDF-1.7\nTest content",
                    await File.ReadAllTextAsync(pdfPath)
                );
            }
            finally
            {
                Directory.Delete(folder, recursive: true);
            }
        } 
    }
}
