//using Xunit;
//using PDFDownloader.Models;
using PDFDownloader.Services;

namespace PDFDownloader.Tests
{
    public class PdfValidatorTests
    {
        [Fact]
        public void IsPdf_ReturnsTrue_ForPdfSignature()
        {
            // Arrange
            string path = Path.GetTempFileName();
            File.WriteAllBytes(path, "%PDF-"u8.ToArray());

            try
            {
                // Act
                var validator = new PdfValidator();
                bool result = validator.IsPdf(path);

                // Assert
                Assert.True(result);
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Fact]
        public void IsPdf_ReturnsFalse_ForNonPdfSignature()
        {
            // Arrange
            string path = Path.GetTempFileName();
            File.WriteAllBytes(path, "Hello world!"u8.ToArray());

            try
            {
                // Act
                var validator = new PdfValidator();
                bool result = validator.IsPdf(path);

                // Assert
                Assert.False(result);
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Fact]
        public void IsPdf_ReturnsFalse_ForEmptyFile()
        {
            // Arrange
            string path = Path.GetTempFileName();
            //File.WriteAllBytes(path, "Hello world!"u8.ToArray());

            try
            {
                // Act
                var validator = new PdfValidator();
                bool result = validator.IsPdf(path);

                // Assert
                Assert.False(result);
            }
            finally
            {
                File.Delete(path);
            }
        }
    }
}