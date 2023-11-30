using System;
namespace ArarararWeb.DataModels
{
    public class FileItem
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public DateTime UploadDate { get; set; }
    }
}