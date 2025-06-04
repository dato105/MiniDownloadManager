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
    public class DownloaderForm2:Form
    {
        private DataService _dataService;             
        private List<FileItem> _fileList;

        public DownloaderForm2(string word)
        {
            InitializeComponent();
           

        }
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DownloaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "DownloaderForm";
            this.Text = "DownloaderForm";
            this.ResumeLayout(false);

        }

        #endregion
    }

}

