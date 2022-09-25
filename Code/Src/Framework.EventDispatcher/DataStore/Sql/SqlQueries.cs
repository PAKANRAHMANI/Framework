using Dapper;
using System.Data;

namespace Framework.EventProcessor.DataStore.Sql
{
    public static class SqlQueries
    {
        public static long GetCursorPosition(this IDbConnection connection, string tableName)
        {
            return connection.Query<long>($"SELECT Position from {tableName}").First();
        }
        public static void MoveCursorPosition(this IDbConnection connection, long position, string tableName)
        {
            connection.Execute($"UPDATE {tableName} SET Position={position}");
        }

        public static List<EventItem> GetEventsFromPosition(this IDbConnection connection, long position, string tableName)
        {
            return connection.Query<EventItem>($"SELECT * from {tableName} WHERE Id > {position}").ToList();
        }
    }
}
