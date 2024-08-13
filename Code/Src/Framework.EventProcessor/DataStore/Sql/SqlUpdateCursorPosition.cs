using Framework.Core.Logging;
using Framework.EventProcessor.Configurations;
using Framework.EventProcessor.DataStore.ChangeTrackers;

namespace Framework.EventProcessor.DataStore.Sql;

public class SqlUpdateCursorPosition(SqlStoreConfig sqlStoreConfig, ILogger logger) : IUpdateCursorPosition
{
    public void MoveCursorPosition(EventItem eventItem)
    {
        SqlQueries.MoveCursorPosition(sqlStoreConfig.ConnectionString, eventItem.Id,
            sqlStoreConfig.CursorTable);

        logger.Write($"cursor moved to : {eventItem.Id}", LogLevel.Information);

    }
}