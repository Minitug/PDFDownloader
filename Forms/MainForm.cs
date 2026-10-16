using PDFDownloader.Models;
using PDFDownloader.Services;

namespace PDFDownloader
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            dgvResults.Columns["ID"].ValueType = typeof(int);

            txtExcelPath.Text = Properties.Settings.Default.LastExcelPath;
            txtOutputFolder.Text = Properties.Settings.Default.LastOutputFolder;
        }

        private bool isDownloading = false;
        private CancellationTokenSource? cancellationTokenSource;

        private void btnBrowseExcel_Click(object sender, EventArgs e)
        {
            using OpenFileDialog dialog = new()
            {
                Filter = "Excel files (*.xlsx)|*.xlsx",
                Title = "Vælg Excel-fil"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtExcelPath.Text = dialog.FileName;
                Properties.Settings.Default.LastExcelPath = dialog.FileName;
                Properties.Settings.Default.Save();
            }
        }

        private void btnBrowseFolder_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog dialog = new()
            {
                Description = "Vælg mappe til PDF-filer"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtOutputFolder.Text = dialog.SelectedPath;
                Properties.Settings.Default.LastOutputFolder = dialog.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }

        private void chkForceDownload_CheckedChanged(object sender, EventArgs e)
        {
            if (chkForceDownload.Checked)
            {
                chkSkipExisting.Checked = false;
            }
        }

        private void chkSkipExisting_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSkipExisting.Checked)
            {
                chkForceDownload.Checked = false;
            }
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            if (isDownloading)
                return;

            isDownloading = true;
            cancellationTokenSource = new CancellationTokenSource();

            CancellationToken cancellationToken = cancellationTokenSource.Token;
            var reportService = new ReportService();
            List<DownloadResult> results = new();
            List<Task<DownloadResult>> downloadTasks = new();
            bool wasCancelled = false;

            btnStart.Enabled = false;
            btnBrowseExcel.Enabled = false;
            btnBrowseFolder.Enabled = false;
            chkForceDownload.Enabled = false;
            chkSkipExisting.Enabled = false;
            btnCancel.Enabled = true;

            try
            {
                try
                {

                    if (!File.Exists(txtExcelPath.Text) ||
                        !string.Equals(Path.GetExtension(txtExcelPath.Text), ".xlsx",
                        StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show(
                            "Excel-filen findes ikke, eller filtypen er ikke .xlsx.",
                            "Ugyldig Excel-fil",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );

                        return;

                    }

                    if (!Directory.Exists(txtOutputFolder.Text))
                    {
                        DialogResult answer = MessageBox.Show(
                            "Outputmappen findes ikke. Vil du oprette den?",
                            "Manglende outputmappe",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question
                        );

                        if (answer == DialogResult.Yes)
                        {
                            Directory.CreateDirectory(txtOutputFolder.Text);
                        }
                        else
                        {
                            return;
                        }
                    }

                    var downloadService = new DownloadService();
                    var excelService = new ExcelService();
                    var excelLines = excelService.ReadExcel(txtExcelPath.Text);
                    dgvResults.Rows.Clear();
                    //int maxDownloads = 50;
                    int succeeded = 0;
                    int skipped = 0;
                    int failed = 0;
                    int maxSimultaneousDownloads = 5;
                    bool SkipExisting = chkSkipExisting.Checked;

                    progressDownloads.Maximum = /*Math.Min(maxDownloads, */excelLines.Count/*)*/;
                    progressDownloads.Value = 0;
                    lblDownloadedMax.Text = $"{progressDownloads.Value.ToString()} / {progressDownloads.Maximum.ToString()}";

                    using var semaphore = new SemaphoreSlim(maxSimultaneousDownloads);

                    async Task<DownloadResult> DownloadWithLimitAsync(DownloadItem row)
                    {
                        await semaphore.WaitAsync(cancellationToken);

                        try
                        {

                            return await downloadService.TryDownloadItemAsync(
                                row,
                                txtOutputFolder.Text,
                                SkipExisting,
                                cancellationToken
                            );
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }

                    var duplicateIds = excelLines
                        .Where(item => int.TryParse(item.Id, out int id) && id > 0)
                        .GroupBy(item => int.Parse(item.Id))
                        .Where(group => group.Count() > 1)
                        .Select(group => group.Key)
                        .ToHashSet();

                    lblDownloadedFiles.Text = succeeded.ToString();
                    lblSkippedFiles.Text = skipped.ToString();
                    lblFailedFiles.Text = failed.ToString();

                    //downloadTasks = new();

                    foreach (var row in excelLines/*.Take(maxDownloads)*/)
                    {
                        cancellationTokenSource.Token.ThrowIfCancellationRequested();
                        if (!int.TryParse(row.Id, out int numericId) || numericId <= 0)
                        {
                            var failedIdResult = new DownloadResult
                            {
                                Id = row.Id,
                                Status = "Fejlet",
                                Url = "",
                                Message = $"BR-nummeret '{row.Id}' skal være et positivt heltal"
                            };


                            results.Add(failedIdResult);

                            dgvResults.Rows.Add(
                                null,
                                failedIdResult.Status,
                                failedIdResult.Url,
                                failedIdResult.Message
                            );

                            failed++;
                            lblFailedFiles.Text = failed.ToString();

                            progressDownloads.Value++;
                            lblDownloadedMax.Text = $"{progressDownloads.Value} / {progressDownloads.Maximum}";

                            continue;
                        }

                        row.Id = numericId.ToString();

                        if (duplicateIds.Contains(numericId))
                        {
                            DownloadResult duplicateIdFailure = new DownloadResult
                            {
                                Id = row.Id,
                                Status = "Fejlet",
                                Url = "",
                                Message = "Dubleret BR-nummer i Excelfilen"
                            };

                            results.Add(duplicateIdFailure);

                            dgvResults.Rows.Add(
                                int.Parse(duplicateIdFailure.Id),
                                duplicateIdFailure.Status,
                                duplicateIdFailure.Url,
                                duplicateIdFailure.Message);

                            failed++;
                            lblFailedFiles.Text = failed.ToString();

                            progressDownloads.Value++;
                            lblDownloadedMax.Text = $"{progressDownloads.Value.ToString()} / {progressDownloads.Maximum.ToString()}";
                            continue;
                        }

                        //DownloadResult result = await DownloadWithLimitAsync(row);
                        downloadTasks.Add(DownloadWithLimitAsync(row));

                    }


                    while (downloadTasks.Count > 0)
                    {
                        Task<DownloadResult> completedTask = await Task.WhenAny(downloadTasks);
                        downloadTasks.Remove(completedTask);

                        try
                        {
                            DownloadResult result = await completedTask;

                            if (result.Status == "Downloadet")
                            {
                                succeeded++;
                                lblDownloadedFiles.Text = succeeded.ToString();
                            }
                            else if (result.Status == "Sprunget over")
                            {
                                skipped++;
                                lblSkippedFiles.Text = skipped.ToString();
                            }
                            else
                            {
                                failed++;
                                lblFailedFiles.Text = failed.ToString();
                            }

                            results.Add(result);

                            dgvResults.Rows.Add(
                                int.Parse(result.Id),
                                result.Status,
                                result.Url,
                                result.Message
                            );

                            progressDownloads.Value++;
                            lblDownloadedMax.Text = $"{progressDownloads.Value.ToString()} / {progressDownloads.Maximum.ToString()}";
                        }
                        catch (OperationCanceledException)
                        {
                            wasCancelled = true;
                        }
                    }
                }

                catch (OperationCanceledException)
                {
                    wasCancelled = true;

                    try
                    {
                        await Task.WhenAll(downloadTasks);
                    }
                    catch (OperationCanceledException)
                    {
                        // Expected when the remaining downloads are cancelled.
                    }

                    MessageBox.Show(
                        "Download blev afbrudt.",
                        "Afbrudt",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                        );
                }

                if (results.Count > 0)
                {
                    reportService.GenerateReport(results, txtOutputFolder.Text, wasCancelled);
                }
            }

            finally
            {
                isDownloading = false;
                btnStart.Enabled = true;
                btnBrowseExcel.Enabled = true;
                btnBrowseFolder.Enabled = true;
                chkForceDownload.Enabled = true;
                chkSkipExisting.Enabled = true;
                btnCancel.Enabled = false;

                cancellationTokenSource?.Dispose();
                cancellationTokenSource = null;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cancellationTokenSource?.Cancel();
        }
        
    }
}
