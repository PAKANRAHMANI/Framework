using System.Text;

namespace Framework.DataAccess.ClickHouse.Connections;

internal class ConnectionStringBuilder
{
    private string _host;
    private int _port;
    private string _username;
    private string _password;
    private bool _compress;
    private bool _checkCompressedHash;
    private string _compressor;
    private int _socketTimeout;
    private string _databaseName;

    public ConnectionStringBuilder()
    {
        this._host = "127.0.0.1";
        this._port = 9000;
        this._username = string.Empty;
        this._password = string.Empty;
        this._compress = true;
        this._checkCompressedHash = false;
        this._compressor = "lz4";
        this._socketTimeout = 120000;
        this._databaseName = "default";
    }
    public ConnectionStringBuilder WithHost(string host)
    {
        this._host = host;
        return this;
    }
    public ConnectionStringBuilder WithPort(int port)
    {
        this._port = port;
        return this;
    }
    public ConnectionStringBuilder WithUsername(string username)
    {
        this._username = username;
        return this;
    }
    public ConnectionStringBuilder WithPassword(string password)
    {
        this._password = password;
        return this;
    }
    public ConnectionStringBuilder WithCompress(bool compress)
    {
        this._compress = compress;
        return this;
    }
    public ConnectionStringBuilder WithCheckCompressedHash(bool checkCompressedHash)
    {
        this._checkCompressedHash = checkCompressedHash;
        return this;
    }
    public ConnectionStringBuilder WithCompressor(string compressor)
    {
        this._compressor = compressor;
        return this;
    }
    /// <summary>
    /// Timeout interval for connecting to the ClickHouse data source
    /// </summary>
    /// <param name="socketTimeout">
    /// Unit: millisecond
    /// </param>
    /// <returns></returns>
    public ConnectionStringBuilder WithSocketTimeout(int socketTimeout)
    {
        this._socketTimeout = socketTimeout;
        return this;
    }
    public ConnectionStringBuilder WithDatabaseName(string databaseName)
    {
        this._databaseName = databaseName;
        return this;
    }

    public string Build()
    {
        return new StringBuilder()
            .Append($"Host={_host};")
            .Append($"Port={_port};")
            .Append($"User={_username};")
            .Append($"Password={_password};")
            .Append($"Compress={_compress};")
            .Append($"CheckCompressedHash={_checkCompressedHash};")
            .Append($"SocketTimeout={_socketTimeout};")
            .Append($"Compressor={_compressor};")
            .Append($"Database={_databaseName}")
            .ToString();
    }
}