namespace PDFDownloader
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlResults = new Panel();
            dgvResults = new DataGridView();
            ID = new DataGridViewTextBoxColumn();
            Status = new DataGridViewTextBoxColumn();
            Source = new DataGridViewTextBoxColumn();
            Message = new DataGridViewTextBoxColumn();
            pnlProgress = new Panel();
            lblDownloadedMax = new Label();
            lblFailedDownloads = new Label();
            lblFailedFiles = new Label();
            lblSkippedDownloads = new Label();
            lblSkippedFiles = new Label();
            lblDownloaded = new Label();
            lblDownloadedFiles = new Label();
            lblProgress = new Label();
            progressDownloads = new ProgressBar();
            pnlControls = new Panel();
            btnCancel = new Button();
            btnStart = new Button();
            pnlSettings = new Panel();
            lblDebug = new Label();
            lblDescription = new Label();
            chkForceDownload = new RadioButton();
            chkSkipExisting = new RadioButton();
            btnBrowseFolder = new Button();
            btnBrowseExcel = new Button();
            lblFileSelection = new Label();
            lblOutputSelection = new Label();
            txtOutputFolder = new TextBox();
            txtExcelPath = new TextBox();
            tlpMain = new TableLayoutPanel();
            pnlResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvResults).BeginInit();
            pnlProgress.SuspendLayout();
            pnlControls.SuspendLayout();
            pnlSettings.SuspendLayout();
            tlpMain.SuspendLayout();
            SuspendLayout();
            // 
            // pnlResults
            // 
            pnlResults.Controls.Add(dgvResults);
            pnlResults.Dock = DockStyle.Fill;
            pnlResults.Location = new Point(3, 415);
            pnlResults.Name = "pnlResults";
            pnlResults.Size = new Size(1440, 217);
            pnlResults.TabIndex = 3;
            // 
            // dgvResults
            // 
            dgvResults.AllowUserToAddRows = false;
            dgvResults.AllowUserToDeleteRows = false;
            dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvResults.Columns.AddRange(new DataGridViewColumn[] { ID, Status, Source, Message });
            dgvResults.Dock = DockStyle.Fill;
            dgvResults.Location = new Point(0, 0);
            dgvResults.Name = "dgvResults";
            dgvResults.ReadOnly = true;
            dgvResults.RowHeadersWidth = 62;
            dgvResults.Size = new Size(1440, 217);
            dgvResults.TabIndex = 0;
            // 
            // ID
            // 
            ID.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            ID.HeaderText = "ID";
            ID.MinimumWidth = 8;
            ID.Name = "ID";
            ID.ReadOnly = true;
            ID.Width = 43;
            // 
            // Status
            // 
            Status.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Status.HeaderText = "Status";
            Status.MinimumWidth = 8;
            Status.Name = "Status";
            Status.ReadOnly = true;
            Status.Width = 64;
            // 
            // Source
            // 
            Source.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            Source.HeaderText = "Kilde_URL";
            Source.MinimumWidth = 8;
            Source.Name = "Source";
            Source.ReadOnly = true;
            Source.Width = 84;
            // 
            // Message
            // 
            Message.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Message.HeaderText = "Besked";
            Message.MinimumWidth = 8;
            Message.Name = "Message";
            Message.ReadOnly = true;
            // 
            // pnlProgress
            // 
            pnlProgress.Controls.Add(lblDownloadedMax);
            pnlProgress.Controls.Add(lblFailedDownloads);
            pnlProgress.Controls.Add(lblFailedFiles);
            pnlProgress.Controls.Add(lblSkippedDownloads);
            pnlProgress.Controls.Add(lblSkippedFiles);
            pnlProgress.Controls.Add(lblDownloaded);
            pnlProgress.Controls.Add(lblDownloadedFiles);
            pnlProgress.Controls.Add(lblProgress);
            pnlProgress.Controls.Add(progressDownloads);
            pnlProgress.Dock = DockStyle.Fill;
            pnlProgress.Location = new Point(3, 259);
            pnlProgress.Name = "pnlProgress";
            pnlProgress.Size = new Size(1440, 150);
            pnlProgress.TabIndex = 2;
            // 
            // lblDownloadedMax
            // 
            lblDownloadedMax.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblDownloadedMax.AutoSize = true;
            lblDownloadedMax.Location = new Point(1395, 17);
            lblDownloadedMax.Name = "lblDownloadedMax";
            lblDownloadedMax.Size = new Size(30, 15);
            lblDownloadedMax.TabIndex = 8;
            lblDownloadedMax.Text = "0 / 0";
            // 
            // lblFailedDownloads
            // 
            lblFailedDownloads.AutoSize = true;
            lblFailedDownloads.Location = new Point(1270, 118);
            lblFailedDownloads.Name = "lblFailedDownloads";
            lblFailedDownloads.Size = new Size(35, 15);
            lblFailedDownloads.TabIndex = 7;
            lblFailedDownloads.Text = "Fejlet";
            // 
            // lblFailedFiles
            // 
            lblFailedFiles.AutoSize = true;
            lblFailedFiles.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblFailedFiles.Location = new Point(1277, 92);
            lblFailedFiles.Name = "lblFailedFiles";
            lblFailedFiles.Size = new Size(19, 21);
            lblFailedFiles.TabIndex = 6;
            lblFailedFiles.Text = "0";
            // 
            // lblSkippedDownloads
            // 
            lblSkippedDownloads.AutoSize = true;
            lblSkippedDownloads.Location = new Point(633, 118);
            lblSkippedDownloads.Name = "lblSkippedDownloads";
            lblSkippedDownloads.Size = new Size(81, 15);
            lblSkippedDownloads.TabIndex = 5;
            lblSkippedDownloads.Text = "Sprunget over";
            // 
            // lblSkippedFiles
            // 
            lblSkippedFiles.AutoSize = true;
            lblSkippedFiles.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSkippedFiles.Location = new Point(663, 92);
            lblSkippedFiles.Name = "lblSkippedFiles";
            lblSkippedFiles.Size = new Size(19, 21);
            lblSkippedFiles.TabIndex = 4;
            lblSkippedFiles.Text = "0";
            // 
            // lblDownloaded
            // 
            lblDownloaded.AutoSize = true;
            lblDownloaded.Location = new Point(65, 118);
            lblDownloaded.Name = "lblDownloaded";
            lblDownloaded.Size = new Size(71, 15);
            lblDownloaded.TabIndex = 3;
            lblDownloaded.Text = "Downloadet";
            // 
            // lblDownloadedFiles
            // 
            lblDownloadedFiles.AutoSize = true;
            lblDownloadedFiles.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDownloadedFiles.Location = new Point(91, 92);
            lblDownloadedFiles.Name = "lblDownloadedFiles";
            lblDownloadedFiles.RightToLeft = RightToLeft.Yes;
            lblDownloadedFiles.Size = new Size(19, 21);
            lblDownloadedFiles.TabIndex = 2;
            lblDownloadedFiles.Text = "0";
            // 
            // lblProgress
            // 
            lblProgress.AutoSize = true;
            lblProgress.BackColor = Color.Transparent;
            lblProgress.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblProgress.Location = new Point(18, 17);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new Size(89, 25);
            lblProgress.TabIndex = 1;
            lblProgress.Text = "Fremdrift";
            // 
            // progressDownloads
            // 
            progressDownloads.Location = new Point(18, 45);
            progressDownloads.Name = "progressDownloads";
            progressDownloads.Size = new Size(1422, 23);
            progressDownloads.TabIndex = 0;
            // 
            // pnlControls
            // 
            pnlControls.Controls.Add(btnCancel);
            pnlControls.Controls.Add(btnStart);
            pnlControls.Dock = DockStyle.Fill;
            pnlControls.Location = new Point(3, 187);
            pnlControls.Name = "pnlControls";
            pnlControls.Size = new Size(1440, 66);
            pnlControls.TabIndex = 1;
            // 
            // btnCancel
            // 
            btnCancel.Enabled = false;
            btnCancel.Location = new Point(1356, 18);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(84, 39);
            btnCancel.TabIndex = 9;
            btnCancel.Text = "Stop";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(18, 18);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(1332, 39);
            btnStart.TabIndex = 0;
            btnStart.Text = "Start downloads";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // pnlSettings
            // 
            pnlSettings.Controls.Add(lblDebug);
            pnlSettings.Controls.Add(lblDescription);
            pnlSettings.Controls.Add(chkForceDownload);
            pnlSettings.Controls.Add(chkSkipExisting);
            pnlSettings.Controls.Add(btnBrowseFolder);
            pnlSettings.Controls.Add(btnBrowseExcel);
            pnlSettings.Controls.Add(lblFileSelection);
            pnlSettings.Controls.Add(lblOutputSelection);
            pnlSettings.Controls.Add(txtOutputFolder);
            pnlSettings.Controls.Add(txtExcelPath);
            pnlSettings.Dock = DockStyle.Fill;
            pnlSettings.Location = new Point(3, 3);
            pnlSettings.Name = "pnlSettings";
            pnlSettings.Size = new Size(1440, 178);
            pnlSettings.TabIndex = 0;
            // 
            // lblDebug
            // 
            lblDebug.AutoSize = true;
            lblDebug.Location = new Point(766, 21);
            lblDebug.Name = "lblDebug";
            lblDebug.Size = new Size(0, 15);
            lblDebug.TabIndex = 9;
            // 
            // lblDescription
            // 
            lblDescription.AutoSize = true;
            lblDescription.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDescription.Location = new Point(18, 9);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(297, 25);
            lblDescription.TabIndex = 8;
            lblDescription.Text = "Download PDF filer fra en Excel fil";
            // 
            // chkForceDownload
            // 
            chkForceDownload.AutoSize = true;
            chkForceDownload.Location = new Point(216, 159);
            chkForceDownload.Name = "chkForceDownload";
            chkForceDownload.Size = new Size(126, 19);
            chkForceDownload.TabIndex = 7;
            chkForceDownload.Text = "Download alle igen";
            chkForceDownload.UseVisualStyleBackColor = true;
            chkForceDownload.CheckedChanged += chkForceDownload_CheckedChanged;
            // 
            // chkSkipExisting
            // 
            chkSkipExisting.AutoSize = true;
            chkSkipExisting.Checked = true;
            chkSkipExisting.Location = new Point(18, 159);
            chkSkipExisting.Name = "chkSkipExisting";
            chkSkipExisting.Size = new Size(192, 19);
            chkSkipExisting.TabIndex = 6;
            chkSkipExisting.TabStop = true;
            chkSkipExisting.Text = "Spring tidligere downloads over";
            chkSkipExisting.UseVisualStyleBackColor = true;
            chkSkipExisting.CheckedChanged += chkSkipExisting_CheckedChanged;
            // 
            // btnBrowseFolder
            // 
            btnBrowseFolder.Location = new Point(1356, 129);
            btnBrowseFolder.Name = "btnBrowseFolder";
            btnBrowseFolder.Size = new Size(84, 23);
            btnBrowseFolder.TabIndex = 5;
            btnBrowseFolder.Text = "Find mappe";
            btnBrowseFolder.UseVisualStyleBackColor = true;
            btnBrowseFolder.Click += btnBrowseFolder_Click;
            // 
            // btnBrowseExcel
            // 
            btnBrowseExcel.Location = new Point(1356, 75);
            btnBrowseExcel.Name = "btnBrowseExcel";
            btnBrowseExcel.Size = new Size(84, 23);
            btnBrowseExcel.TabIndex = 4;
            btnBrowseExcel.Text = "Gennemse";
            btnBrowseExcel.UseVisualStyleBackColor = true;
            btnBrowseExcel.Click += btnBrowseExcel_Click;
            // 
            // lblFileSelection
            // 
            lblFileSelection.AutoSize = true;
            lblFileSelection.ForeColor = SystemColors.ControlText;
            lblFileSelection.Location = new Point(18, 58);
            lblFileSelection.Name = "lblFileSelection";
            lblFileSelection.Size = new Size(261, 15);
            lblFileSelection.TabIndex = 3;
            lblFileSelection.Text = "Specificer venligst Excelfilen med downloadlinks";
            // 
            // lblOutputSelection
            // 
            lblOutputSelection.AutoSize = true;
            lblOutputSelection.ForeColor = SystemColors.ControlText;
            lblOutputSelection.Location = new Point(18, 112);
            lblOutputSelection.Name = "lblOutputSelection";
            lblOutputSelection.Size = new Size(233, 15);
            lblOutputSelection.TabIndex = 2;
            lblOutputSelection.Text = "Specificer venligst download destinationen";
            // 
            // txtOutputFolder
            // 
            txtOutputFolder.Location = new Point(18, 130);
            txtOutputFolder.Name = "txtOutputFolder";
            txtOutputFolder.ReadOnly = true;
            txtOutputFolder.Size = new Size(1332, 23);
            txtOutputFolder.TabIndex = 1;
            // 
            // txtExcelPath
            // 
            txtExcelPath.Location = new Point(18, 76);
            txtExcelPath.Name = "txtExcelPath";
            txtExcelPath.ReadOnly = true;
            txtExcelPath.Size = new Size(1332, 23);
            txtExcelPath.TabIndex = 0;
            // 
            // tlpMain
            // 
            tlpMain.ColumnCount = 1;
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpMain.Controls.Add(pnlControls, 0, 1);
            tlpMain.Controls.Add(pnlProgress, 0, 2);
            tlpMain.Controls.Add(pnlResults, 0, 3);
            tlpMain.Controls.Add(pnlSettings, 0, 0);
            tlpMain.Dock = DockStyle.Fill;
            tlpMain.Location = new Point(0, 0);
            tlpMain.MaximumSize = new Size(1446, 1200);
            tlpMain.MinimumSize = new Size(1446, 635);
            tlpMain.Name = "tlpMain";
            tlpMain.RowCount = 4;
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 184F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 156F));
            tlpMain.RowStyles.Add(new RowStyle());
            tlpMain.Size = new Size(1446, 635);
            tlpMain.TabIndex = 9;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1448, 635);
            Controls.Add(tlpMain);
            MaximumSize = new Size(1468, 1193);
            MinimumSize = new Size(1347, 634);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PDF Downloader";
            pnlResults.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvResults).EndInit();
            pnlProgress.ResumeLayout(false);
            pnlProgress.PerformLayout();
            pnlControls.ResumeLayout(false);
            pnlSettings.ResumeLayout(false);
            pnlSettings.PerformLayout();
            tlpMain.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Panel pnlSettings;
        private Panel pnlProgress;
        private Panel pnlControls;
        private Panel pnlResults;
        private Label lblOutputSelection;
        private TextBox txtOutputFolder;
        private TextBox txtExcelPath;
        private Button btnBrowseFolder;
        private Button btnBrowseExcel;
        private Label lblFileSelection;
        private RadioButton chkForceDownload;
        private RadioButton chkSkipExisting;
        private Button btnStart;
        private Label lblDescription;
        private Button btnCancel;
        private Label lblDownloaded;
        private Label lblDownloadedFiles;
        private Label lblProgress;
        private ProgressBar progressDownloads;
        private Label lblSkippedFiles;
        private Label lblSkippedDownloads;
        private Label lblFailedDownloads;
        private Label lblFailedFiles;
        private DataGridView dgvResults;
        private TableLayoutPanel tlpMain;
        private Label lblDebug;
        private Label lblDownloadedMax;
        private DataGridViewTextBoxColumn ID;
        private DataGridViewTextBoxColumn Status;
        private DataGridViewTextBoxColumn Source;
        private DataGridViewTextBoxColumn Message;
    }
}
