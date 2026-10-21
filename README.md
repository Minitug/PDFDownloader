# PDF Downloader

A Windows desktop application developed in **C# using .NET 10 and Windows Forms** that automatically downloads PDF documents from URLs provided in an Excel spreadsheet.

The application was developed as a course project, focusing on file handling, HTTP communication, asynchronous programming, parallel downloads, error handling, and automated testing.

## Features

- **Excel import:** Read document IDs, primary URLs, and alternative URLs from an Excel file.
- **Parallel downloads:** Download multiple PDF files simultaneously using asynchronous operations.
- **Fallback URLs:** Automatically attempt an alternative URL if the primary URL fails.
- **PDF validation:** Verify the PDF file signature (`%PDF-`) before accepting a downloaded file.
- **Skip existing files:** Avoid downloading documents again when a valid PDF already exists.
- **Force redownload:** Optionally replace previously downloaded files.
- **Cancellation:** Cancel ongoing download operations.
- **Progress tracking:** Display download progress and counts of successful, failed, and skipped files.
- **Excel reporting:** Generate a report containing download results and error messages.
- **Error handling:** Handle invalid document IDs, duplicate IDs, HTTP errors, timeouts, and unsuccessful downloads.

## Technologies

- C#
- .NET 10
- Windows Forms (WinForms)
- ClosedXML for reading and writing Excel files
- HttpClient for HTTP requests
- xUnit for automated testing

## Requirements

- Windows
- .NET 10 SDK for building from source
- Visual Studio with the .NET desktop development workload (recommended)

## Getting Started

1. Clone the repository:

   ```bash
   git clone https://github.com/Minitug/PDFDownloader.git
   ```

2. Open the project in Visual Studio.
3. Restore the NuGet packages.
4. Build and run the application.

Alternatively, run the project from the command line:

```bash
dotnet run --project PDFDownloader.csproj
```

The application is Windows-only because it uses Windows Forms.

## Excel Input Format

The input Excel file must contain three columns:

| Column | Content | Description |
|---|---|---|
| A | Document ID (BR number) | Unique identifier used to name the downloaded PDF |
| B | Primary URL | First download location |
| C | Alternative URL | Fallback location if the primary URL fails |

Example:

| ID | Primary URL | Alternative URL |
|---|---|---|
| 123 | https://example.com/123.pdf | https://example.com/backup/123.pdf |
| 124 | https://example.com/124.pdf | |
| 125 | | https://example.com/125.pdf |

The first row is treated as a header row.

## How It Works

1. Select an Excel file containing document IDs and URLs.
2. Select an output folder for the downloaded PDFs.
3. Choose whether to skip existing valid PDFs or download them again.
4. Start the download process.
5. Monitor progress and results in the application.
6. Open the generated Excel report to review the results.

Downloaded files are named using their document IDs:

```text
123.pdf
124.pdf
125.pdf
```

All files are saved directly in the selected output folder.

## Download Process

For each Excel row, the application:

1. Validates the document ID and checks for duplicates.
2. Checks whether a valid PDF already exists when skipping is enabled.
3. Attempts to download the document from the primary URL.
4. Tries the alternative URL if the primary attempt fails.
5. Saves downloaded data to a temporary `.pdf.tmp` file.
6. Validates the file's PDF signature.
7. Renames valid files to `{ID}.pdf`.
8. Records the result for the user interface and Excel report.

Downloads run asynchronously, with a `SemaphoreSlim` limiting the number of simultaneous operations.

## Reporting

After processing, the application generates an Excel report containing:

- Document ID
- Download status in Danish (`Downloadet (downloaded)`, `Fejlet(failed)`, or `Sprunget over(skipped)`)
- URL
- Result message or error details

If both the primary and alternative URLs fail, the report preserves error information from both attempts.

Results are sorted numerically by document ID in the Excel report.

If a download session is cancelled, a partial report can be generated for the results collected before cancellation.

## Project Structure

```text
PDFDownloader/
├── Forms/
│   ├── MainForm.cs
│   ├── MainForm.Designer.cs
│   └── MainForm.resx
├── Models/
│   ├── DownloadItem.cs
│   └── DownloadResult.cs
├── Services/
│   ├── DownloadService.cs
│   ├── ExcelService.cs
│   ├── PdfValidator.cs
│   └── ReportService.cs
├── Properties/
│   ├── Settings.Designer.cs
│   └── Settings.settings
├── PDFDownloader.Tests/
│   ├── DownloadServiceTests.cs
│   ├── ExcelServiceTests.cs
│   ├── PdfValidatorTests.cs
│   └── PDFDownloader.Tests.csproj
├── Program.cs
├── App.config
├── AssemblyInfo.cs
├── PDFDownloader.csproj
├── PDFDownloader.slnx
└── README.md
```

### Main Components

| Component | Responsibility |
|---|---|
| `MainForm` | User interface, progress tracking, and cancellation |
| `ExcelService` | Reading and processing Excel input |
| `DownloadService` | HTTP requests, downloads, and fallback URLs |
| `PdfValidator` | PDF file signature validation |
| `ReportService` | Excel report generation |
| `DownloadItem` | Input data for individual documents |
| `DownloadResult` | Results of download attempts |

## Automated Tests

The solution includes a separate xUnit test project: `PDFDownloader.Tests`.

Tests cover functionality such as:

- Valid and invalid PDF signatures.
- Reading Excel data and skipping empty rows.
- Handling missing URLs.
- Skipping existing valid PDF files.
- Replacing invalid existing files.
- Falling back to alternative URLs.
- Handling unsuccessful HTTP responses.
- Distinguishing HTTP timeouts from user cancellation.

HTTP responses can be simulated using a custom `HttpMessageHandler`, allowing download tests to run without relying on external websites.

Run the tests using Visual Studio's **Test Explorer** or:

```bash
dotnet test
```

## Limitations

Not every PDF can necessarily be downloaded successfully.

External servers may return errors such as `403 Forbidden`, `404 Not Found`, or request timeouts. Some websites restrict automated downloads or require authentication.

The application attempts alternative URLs when available and records failures for further investigation.

PDF validation checks the file signature but does not perform a complete structural validation of the PDF document.

## Project Background

This application was created as a school project to gain practical experience with:

- Object-oriented programming and separation of responsibilities.
- Asynchronous and parallel programming.
- HTTP communication and error handling.
- File and Excel processing.
- Automated testing with xUnit.
- Version control using Git and GitHub.

The project is intended for educational purposes.
