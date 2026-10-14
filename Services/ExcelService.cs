using System;
using System.Collections.Generic;
using System.Text;
using ClosedXML.Excel;
using PDFDownloader.Models;

namespace PDFDownloader.Services;

public class ExcelService
{
    internal List<DownloadItem> ReadExcel(string filePath)
    {
        var items = new List<DownloadItem>();

        using var workbook = new XLWorkbook(filePath);
        var worksheet = workbook.Worksheet(1);

        // Row 1 contains column headers.
        foreach (var row in worksheet.RowsUsed().Skip(1))
        {
            string id = row.Cell(1).GetFormattedString().Trim();
            string url = row.Cell(2).GetString().Trim();
            string altUrl = row.Cell(3).GetString().Trim();

            // Ignore completely empty rows.
            if (string.IsNullOrWhiteSpace(id) &&
                string.IsNullOrWhiteSpace(url) &&
                string.IsNullOrWhiteSpace(altUrl))
            {
                continue;
            }

            items.Add(new DownloadItem
            {
                Id = id,
                Url = url,
                AltUrl = altUrl
            });
        }

        return items;
    }
}
