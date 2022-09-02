namespace Framework.FileUpload
{
    public class FileUpload
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string Extension { get; set; }
        public byte[] Data { get; set; }
    }
}
