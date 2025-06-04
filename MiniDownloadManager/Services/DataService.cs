using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MiniDownloadManager.Models;

namespace MiniDownloadManager.Services
{
    public class DataService
    {
        private readonly string _jsonUrl;
        private readonly HttpClient _httpClient;

        public DataService(string url)
        {
            _jsonUrl = url;
            _httpClient = new HttpClient();
        }

        /// Fetch JSON data asynchronously and parse into a list of FileItem.
        public async Task<List<FileItem>> FetchFileItemsAsync()
        {
            try
            {
                string jsonString = await _httpClient.GetStringAsync(_jsonUrl);
                var files = JsonConvert.DeserializeObject<List<FileItem>>(jsonString);
                return files;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching data:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new List<FileItem>();
            }
        }

        
        public FileItem GetHighestScoreFile(List<FileItem> files)
        {
            if (files == null || files.Count == 0)
                return null;

            return files.OrderByDescending(f => f.Score).FirstOrDefault();
        }

    
        /// Download a file from URL to the temp directory.
        /// Returns the local file path or null if failed.
        public async Task<string> DownloadFileAsync(string fileUrl)
        {
            try
            {
                string filename = Path.GetFileName(new Uri(fileUrl).LocalPath);
                string downloadFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MiniDownloadManager");
                Directory.CreateDirectory(downloadFolder);  // Ensure the folder exists

                string localPath = Path.Combine(downloadFolder, filename);

                // Check if file already exists
                if (File.Exists(localPath))
                {
                    var result = MessageBox.Show($"The file \"{filename}\" has already been downloaded. Do you want to open it?",
                        "File Already Downloaded", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        // Open the folder and execute the file
                        System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{localPath}\"");
                    }
                    return localPath;
                }

                // Download if not existing
                var response = await _httpClient.GetAsync(fileUrl);
                response.EnsureSuccessStatusCode();

                byte[] data = await response.Content.ReadAsByteArrayAsync();
                await File.WriteAllBytesAsync(localPath, data);

                // Open folder after download and execute the file
                System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{localPath}\"");

                return localPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to download file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}