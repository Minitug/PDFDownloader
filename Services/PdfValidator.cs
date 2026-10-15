using System.Text;

namespace PDFDownloader.Services
{
    internal class PdfValidator
    {
        internal bool IsPdf(string filePath)
        {
            using var readingFile = File.OpenRead(filePath);
            using var reader = new BinaryReader(readingFile);

            byte[] bytes = reader.ReadBytes(5);
            string fileSignature = Encoding.ASCII.GetString(bytes);

            return fileSignature == "%PDF-";
        }
    }
}
