using PDFDownloader.Models;
using PDFDownloader.Services;
using System.Net;
using System.Text;
using Xunit;


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

        [Fact]
        public async Task TryDownloadItemAsync_InvalidExistingPdf_DownloadsNewPdf()
        {
            string folder = CreateTempFolder();

            try
            {
                string pdfPath = Path.Combine(folder, "123.pdf");

                await File.WriteAllTextAsync(pdfPath, "Not a PDF");

                var handler = new FakeHttpMessageHandler((request, token) =>
                    Task.FromResult(PdfResponse()));

                using var client = new HttpClient(handler);
                var service = new DownloadService(client);

                var item = new DownloadItem
                {
                    Id = "123",
                    Url = "https://example.com/123.pdf",
                    AltUrl = ""
                };

                DownloadResult result = await service.TryDownloadItemAsync(
                    item, folder, true, CancellationToken.None);

                Assert.Equal("Downloadet", result.Status);
                Assert.StartsWith("%PDF-", await File.ReadAllTextAsync(pdfPath));
                Assert.Equal(1, handler.RequestCount);
            }
            finally
            {
                Directory.Delete(folder, recursive: true);
            }
        }

        [Fact]
        public async Task TryDownloadItemAsync_PrimaryFails_UsesAlternativeUrl()
        {
            string folder = CreateTempFolder();

            try
            {
                string primaryUrl = "https://example.com/primary.pdf";
                string alternativeUrl = "https://example.com/alternative.pdf";

                var handler = new FakeHttpMessageHandler((request, token) =>
                {
                    if (request.RequestUri!.ToString() == primaryUrl)
                    {
                        return Task.FromResult(
                            new HttpResponseMessage(HttpStatusCode.Forbidden));
                    }

                    return Task.FromResult(PdfResponse());
                });

                using var client = new HttpClient(handler);
                var service = new DownloadService(client);

                var item = new DownloadItem
                {
                    Id = "123",
                    Url = primaryUrl,
                    AltUrl = alternativeUrl
                };

                DownloadResult result = await service.TryDownloadItemAsync(
                    item, folder, false, CancellationToken.None);

                Assert.Equal("Downloadet", result.Status);
                Assert.Equal(alternativeUrl, result.Url);
                Assert.Equal(2, handler.RequestCount);
                Assert.True(File.Exists(Path.Combine(folder, "123.pdf")));
            }
            finally
            {
                Directory.Delete(folder, recursive: true);
            }
        }

        [Fact]
        public async Task TryDownloadItemAsync_BothUrlsFail_ReturnsFailed()
        {
            string folder = CreateTempFolder();

            try
            {
                var handler = new FakeHttpMessageHandler((request, token) =>
                {
                    HttpStatusCode status =
                        request.RequestUri!.AbsolutePath.Contains("primary")
                            ? HttpStatusCode.Forbidden
                            : HttpStatusCode.NotFound;

                    return Task.FromResult(new HttpResponseMessage(status));
                });

                using var client = new HttpClient(handler);
                var service = new DownloadService(client);

                var item = new DownloadItem
                {
                    Id = "123",
                    Url = "https://example.com/primary.pdf",
                    AltUrl = "https://example.com/alternative.pdf"
                };

                DownloadResult result = await service.TryDownloadItemAsync(
                    item, folder, false, CancellationToken.None);

                Assert.Equal("Fejlet", result.Status);
                Assert.Equal(2, handler.RequestCount);
                Assert.False(File.Exists(Path.Combine(folder, "123.pdf")));
            }
            finally
            {
                Directory.Delete(folder, recursive: true);
            }
        }

        [Fact]
        public async Task TryDownloadItemAsync_HttpTimeout_ReturnsFailedResult()
        {
            string folder = CreateTempFolder();

            try
            {
                var handler = new FakeHttpMessageHandler((request, token) =>
                    throw new TaskCanceledException("Simulated HTTP timeout"));

                using var client = new HttpClient(handler);
                var service = new DownloadService(client);

                var item = new DownloadItem
                {
                    Id = "123",
                    Url = "https://example.com/primary.pdf",
                    AltUrl = "https://example.com/alternative.pdf"
                };

                DownloadResult result = await service.TryDownloadItemAsync(
                    item, folder, false, CancellationToken.None);

                Assert.Equal("Fejlet", result.Status);
                Assert.Contains("timeout", result.Message, StringComparison.OrdinalIgnoreCase);
                Assert.Equal(2, handler.RequestCount);
            }
            finally
            {
                Directory.Delete(folder, recursive: true);
            }
        }

        private static string CreateTempFolder()
        {
            string folder = Path.Combine(
                Path.GetTempPath(),
                Guid.NewGuid().ToString());

            Directory.CreateDirectory(folder);

            return folder;
        }

        private static HttpResponseMessage PdfResponse()
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(
                    Encoding.ASCII.GetBytes("%PDF-1.7\nTest PDF content"))
            };
        }

        private sealed class FakeHttpMessageHandler : HttpMessageHandler
        {
            private readonly Func<
                HttpRequestMessage,
                CancellationToken,
                Task<HttpResponseMessage>> responseFactory;

            private int requestCount;

            public int RequestCount => Volatile.Read(ref requestCount);

            public FakeHttpMessageHandler(
                Func<HttpRequestMessage, CancellationToken,
                    Task<HttpResponseMessage>> responseFactory)
            {
                this.responseFactory = responseFactory;
            }

            protected override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                Interlocked.Increment(ref requestCount);

                return responseFactory(request, cancellationToken);
            }
        }
    }
}
