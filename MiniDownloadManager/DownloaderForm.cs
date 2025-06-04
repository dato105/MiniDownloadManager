using MiniDownloadManager.Models;
using MiniDownloadManager.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiniDownloadManager
{
    public partial class DownloaderForm : Form
    {
        private DataService _dataService;
        private List<FileItem> _fileList;
        private FileItem _selectedFile;
        private Label lblTitle;
        private Label lblFileName;
        private PictureBox pictureBox;
        private Button btnRefresh;
        private Button btnDownload;

        public DownloaderForm()
        {
            InitializeComponent();
            this.Load += DownloaderForm_Load;
        }
        private async void DownloaderForm_Load(object sender, EventArgs e)
        {
            // Your data loading logic goes here
            string url = "https://4qgz7zu7l5um367pzultcpbhmm0thhhg.lambda-url.us-west-2.on.aws/"; // Replace with your actual URL
            _dataService = new DataService(url);
            _fileList = await _dataService.FetchFileItemsAsync();

            if (_fileList != null && _fileList.Count > 0)
            {
                // Select the highest score file
                _selectedFile = _dataService.GetHighestScoreFile(_fileList);

                // Update UI with selected file
                UpdateUIWithFile(_selectedFile);
            }
            else
            {
                MessageBox.Show("No files available", "Information");
            }
        }


        private async void UpdateUIWithFile(FileItem file)
        {
            if (file != null)
            {
                lblFileName.Text = file.Title;

                // Load image from URL asynchronously
                if (!string.IsNullOrEmpty(file.ImageURL))
                {
                    try
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            byte[] imageBytes = await httpClient.GetByteArrayAsync(file.ImageURL);
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                pictureBox.Image = System.Drawing.Image.FromStream(ms);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    pictureBox.Image = null; // Clear the image if no URL is available
                }
            }
        }


        private async void btnDownload_Click(object sender, EventArgs e)
        {
            if (_selectedFile != null)
            {
                string localPath = await _dataService.DownloadFileAsync(_selectedFile.FileURL);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {

            if (_fileList != null && _fileList.Count > 0)
            {
                // Find the file with the next highest score
                int currentIndex = _fileList.IndexOf(_selectedFile);
                if (currentIndex >= 0)
                {
                    currentIndex = (currentIndex + 1) % _fileList.Count; // Cycle through the list

                    _selectedFile = _fileList[currentIndex];
                    UpdateUIWithFile(_selectedFile);
                }
            }
        }



        private void InitializeComponent()
        {
            this.btnDownload = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblFileName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // btnDownload
            // 
            this.btnDownload.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.btnDownload.Location = new System.Drawing.Point(414, 335);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(100, 34);
            this.btnDownload.TabIndex = 0;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblTitle.Location = new System.Drawing.Point(347, 57);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(363, 37);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Mini-Download Manager";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(363, 194);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(200, 100);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 2;
            this.pictureBox.TabStop = false;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(424, 385);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblFileName.Location = new System.Drawing.Point(432, 140);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(0, 25);
            this.lblFileName.TabIndex = 4;
            this.lblFileName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DownloaderForm
            // 
            this.ClientSize = new System.Drawing.Size(976, 644);
            this.Controls.Add(this.lblFileName);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnDownload);
            this.Name = "DownloaderForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    


    }
}
