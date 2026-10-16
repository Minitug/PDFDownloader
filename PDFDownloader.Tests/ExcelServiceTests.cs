using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using PDFDownloader.Models;
using PDFDownloader.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace PDFDownloader.Tests
{
    public class ExcelServiceTests
    {
        [Fact]
        public void ReadExcel_ReturnsDownloadItem_ForValidRow()
        {
            string path = Path.Combine(
                Path.GetTempPath(),
                $"{Guid.NewGuid()}.xlsx"
            );

            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add("Test");

                    sheet.Cell("A1").Value = "id";
                    sheet.Cell("B1").Value = "url";
                    sheet.Cell("C1").Value = "alt url";

                    sheet.Cell("A2").Value = "123";
                    sheet.Cell("B2").Value = "https://example.com/123.pdf";
                    sheet.Cell("C2").Value = "https://example.com/backup.pdf";

                    workbook.SaveAs(path);
                }

                var excelService = new ExcelService();
                var excelLines = excelService.ReadExcel(path);
                Assert.Single(excelLines);
                Assert.Equal("123", excelLines[0].Id);
                Assert.Equal("https://example.com/123.pdf", excelLines[0].Url);
                Assert.Equal("https://example.com/backup.pdf", excelLines[0].AltUrl);
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Fact]
        public void ReadExcel_SkipsEmptyRows()
        {
            string path = Path.Combine(
                Path.GetTempPath(),
                $"{Guid.NewGuid()}.xlsx"
            );

            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add("Test");

                    sheet.Cell("A1").Value = "id";
                    sheet.Cell("B1").Value = "url";
                    sheet.Cell("C1").Value = "alt url";

                    sheet.Cell("A2").Value = "1";
                    sheet.Cell("B2").Value = "https://example.com/123.pdf";
                    sheet.Cell("C2").Value = "https://example.com/backup.pdf";

                    sheet.Cell("A3").Value = "   ";
                    sheet.Cell("B3").Value = " ";
                    sheet.Cell("C3").Value = "";

                    sheet.Cell("A4").Value = "3";
                    sheet.Cell("B4").Value = "https://example.com/425.pdf";
                    sheet.Cell("C4").Value = "https://example.com/backup222.pdf";

                    workbook.SaveAs(path);
                }

                var excelService = new ExcelService();
                var excelLines = excelService.ReadExcel(path);
                Assert.Equal(2, excelLines.Count);
                Assert.Equal("1", excelLines[0].Id);
                Assert.Equal("https://example.com/123.pdf", excelLines[0].Url);
                Assert.Equal("https://example.com/backup.pdf", excelLines[0].AltUrl);
                Assert.Equal("3", excelLines[1].Id);
                Assert.Equal("https://example.com/425.pdf", excelLines[1].Url);
                Assert.Equal("https://example.com/backup222.pdf", excelLines[1].AltUrl);
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Fact]
        public void ReadExcel_PreservesInvalidId()
        {
            string path = Path.Combine(
                Path.GetTempPath(),
                $"{Guid.NewGuid()}.xlsx"
            );

            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var sheet = workbook.Worksheets.Add("Test");

                    sheet.Cell("A1").Value = "id";
                    sheet.Cell("B1").Value = "url";
                    sheet.Cell("C1").Value = "alt url";

                    sheet.Cell("A2").Value = "12a";
                    sheet.Cell("B2").Value = "https://example.com/123.pdf";
                    sheet.Cell("C2").Value = "https://example.com/backup.pdf";

                    workbook.SaveAs(path);
                }

                var excelService = new ExcelService();
                var excelLines = excelService.ReadExcel(path);
                Assert.Single(excelLines);
                Assert.Equal("12a", excelLines[0].Id);

            }
            finally
            {
                File.Delete(path);
            }
        }
    }
}
