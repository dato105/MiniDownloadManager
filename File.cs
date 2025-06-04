namespace MiniDownloadManager.Models
{
    public class Validator
    {

        public int disk { get; set; }
        public int os { get; set; }
        public int ram { get; set; }

        public Validator(int diskParam , int osParam , int ramParam )
        {
            this.disk = diskParam;
            this.os = osParam;
            this.ram = ramParam;
        }
    }

    public class File
    {
        public string Title { get; set; }
        public string ImageURL { get; set; }
        public string FileURL { get; set; }
        public int Score { get; set; }
        public Validator Validator { get; set; }
    }
   
}