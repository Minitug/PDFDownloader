using PDFDownloader.Models;
using PDFDownloader.Services;
using System.Net.Http;
using System.Runtime.Intrinsics.X86;
using System.Security.Policy;

namespace PDFDownloader
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            txtExcelPath.Text = Properties.Settings.Default.LastExcelPath;
            txtOutputFolder.Text = Properties.Settings.Default.LastOutputFolder;
        }

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
            int maxDownloads = 10;
            int succeeded = 0;
            int skipped = 0;
            int failed = 0;
            bool SkipExisting = chkSkipExisting.Checked;

            var duplicateIds = excelLines
                .GroupBy(item => item.Id)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key)
                .ToHashSet();

            List<DownloadResult> results = new();

            lblDownloadedFiles.Text = succeeded.ToString();
            lblSkippedFiles.Text = skipped.ToString();
            lblFailedFiles.Text = failed.ToString();

            foreach (var row in excelLines.Take(maxDownloads))
            {
                if (duplicateIds.Contains(row.Id))
                {
                    DownloadResult duplicateIdFailure = new DownloadResult {
                        Id = row.Id,
                        Status = "Fejlet",
                        Url = "",
                        Message = "Dubleret BR-nummer i Excelfilen"
                    };

                    results.Add(duplicateIdFailure);

                    dgvResults.Rows.Add(
                        duplicateIdFailure.Id,
                        duplicateIdFailure.Status,
                        duplicateIdFailure.Url,
                        duplicateIdFailure.Message);

                    failed++;
                    lblFailedFiles.Text = failed.ToString();

                    continue;
                }

                DownloadResult result = await downloadService.TryDownloadItemAsync(
                    row,
                    txtOutputFolder.Text,
                    SkipExisting
                );
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
                    result.Id,
                    result.Status,
                    result.Url,
                    result.Message
                );

            }
        }
    }
}
