namespace Framework.DataAccess.ClickHouse.Migrator;

public class ClickHouseConfigurationBuilder
{
    private string _host;
    private int _port;
    private string _username;
    private string _password;
    private string _databaseName;
    private int _commandTimeout;
    private int _readWriteTimeout;

    public ClickHouseConfigurationBuilder WithHost(string host)
    {
        this._host = host;
        return this;
    }
    public ClickHouseConfigurationBuilder WithPort(int port)
    {
        this._port = port;
        return this;
    }
    public ClickHouseConfigurationBuilder WithUsername(string username)
    {
        this._username = username;
        return this;
    }
    public ClickHouseConfigurationBuilder WithPassword(string password)
    {
        this._password = password;
        return this;
    }
    public ClickHouseConfigurationBuilder WithDatabaseName(string databaseName)
    {
        this._databaseName = databaseName;
        return this;
    }
    public ClickHouseConfigurationBuilder WithCommandTimeout(int commandTimeout)
    {
        this._commandTimeout = commandTimeout;
        return this;
    }
    public ClickHouseConfigurationBuilder WithReadWriteTimeout(int readWriteTimeout)
    {
        this._readWriteTimeout = readWriteTimeout;
        return this;
    }

    public ClickHouseConfiguration Build()
    {
        return new ClickHouseConfiguration
        {
            CommandTimeout = _commandTimeout,
            ReadWriteTimeout = _readWriteTimeout,
            Database = _databaseName,
            Host = _host,
            Password = _password,
            Port = _port,
            Username = _username
        };
    }
}