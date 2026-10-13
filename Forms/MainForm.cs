namespace PDFDownloader
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
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
            }
        }


        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            
        }


    }

}
