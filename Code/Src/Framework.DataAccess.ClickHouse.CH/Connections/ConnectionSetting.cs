namespace Framework.DataAccess.CH.Connections
{
    public class ConnectionSetting
    {
        public string Host { get; set; }
        public string DatabaseName { get; set; }
        public int Port { get; set; }
        public int SocketTimeout { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Compressor { get; set; }
        public bool Compress { get; set; }
        public bool CheckCompressedHash { get; set; }
    }
}
