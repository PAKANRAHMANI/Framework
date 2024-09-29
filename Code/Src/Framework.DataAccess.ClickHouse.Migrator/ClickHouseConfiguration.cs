namespace Framework.DataAccess.ClickHouse.Migrator
{
    public class ClickHouseConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
        public int CommandTimeout { get; set; }
        public int ReadWriteTimeout { get; set; }
    }
}
