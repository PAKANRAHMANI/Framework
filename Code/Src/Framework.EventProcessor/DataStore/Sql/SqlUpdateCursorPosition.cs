using Framework.Core.Logging;
using Framework.EventProcessor.Configurations;
using Framework.EventProcessor.DataStore.ChangeTrackers;
using Microsoft.Extensions.Options;

namespace Framework.EventProcessor.DataStore.Sql;

public class SqlUpdateCursorPosition: IUpdateCursorPosition
{
    private readonly SqlStoreConfig _sqlStoreConfig;
    private readonly ILogger _logger;

    public SqlUpdateCursorPosition(IOptions<SqlStoreConfig> sqlStoreConfig, ILogger logger)
    {
        _sqlStoreConfig = sqlStoreConfig.Value;
        _logger = logger;
    }
    public void MoveCursorPosition(EventItem eventItem)
    {
        SqlQueries.MoveCursorPosition(_sqlStoreConfig.ConnectionString, eventItem.Id,
            _sqlStoreConfig.CursorTable);

        _logger.Write($"cursor moved to : {eventItem.Id}", LogLevel.Information);

    }
}