using ClosedXML.Excel;
using PDFDownloader.Models;

namespace PDFDownloader.Services
{
    internal class ReportService
    {
        internal void GenerateReport(
            List<DownloadResult> results,
            string outputFolder
        )
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string reportPath = Path.Combine(outputFolder, $"PDF_Downloader_rapport_{timestamp}.xlsx");

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("rapport");

            int rowNumber = 1;

            worksheet.Cell($"A{rowNumber}").Value = "ID";
            worksheet.Cell($"B{rowNumber}").Value = "Status";
            worksheet.Cell($"C{rowNumber}").Value = "Kilde_URL";
            worksheet.Cell($"D{rowNumber}").Value = "Besked";

            rowNumber++;

            foreach (var row in results){
                worksheet.Cell(rowNumber, 1).Value = row.Id;
                worksheet.Cell(rowNumber, 2).Value = row.Status;
                worksheet.Cell(rowNumber, 3).Value = row.Url;
                worksheet.Cell(rowNumber, 4).Value = row.Message;

                rowNumber++;
            }

            worksheet.Row(1).Style.Font.Bold = true;
            worksheet.Columns().AdjustToContents();
            workbook.SaveAs(reportPath);
        }
    }
}
