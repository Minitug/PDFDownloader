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
            var downloadService = new DownloadService();
            var excelService = new ExcelService();
            //var ExcelLines = new List<DownloadItem>();
            var excelLines = excelService.ReadExcel(txtExcelPath.Text);
            dgvResults.Rows.Clear();
            int maxDownloads = 10;
            //int i = 1;
            int succeeded = 0;
            int skipped = 0; 
            int failed = 0;

            foreach (var row in excelLines.Take(maxDownloads))
            {
                DownloadResult result = await downloadService.TryDownloadItemAsync(
                    row,
                    txtOutputFolder.Text
                );
                if (result.Status == "Downloadet")
                {
                    succeeded++;
                    lblDownloadedFiles.Text = succeeded.ToString();
                }
                else
                {
                    skipped++;
                    lblFailedFiles.Text = skipped.ToString();
                }

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
