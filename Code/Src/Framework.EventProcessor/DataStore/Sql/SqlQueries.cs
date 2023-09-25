using Microsoft.Data.SqlClient;
using System.Transactions;

namespace Framework.EventProcessor.DataStore.Sql
{
    public static class SqlQueries
    {
        public static long GetCursorPosition(string connectionString, string tableName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var commandText = $"SELECT Position from {tableName}";

                var command = new SqlCommand(commandText, connection);

                connection.Open();

                return Convert.ToInt64(command.ExecuteScalar());
            }
        }
        public static void MoveCursorPosition(string connectionString, long position, string tableName)
        {
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.MaximumTimeout
            };

            using (var scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    var commandText = $"UPDATE {tableName} SET Position={position}";

                    var command = new SqlCommand(commandText, connection);

                    connection.Open();

                    command.ExecuteNonQuery();

                    scope.Complete();

                    connection.Close();
                }
            }
        }

        public static List<EventItem> GetEventsFromPosition(string connectionString, long position, string tableName)
        {
            var items = new List<EventItem>();
            using (var connection = new SqlConnection(connectionString))
            {
                var commandText = $"SELECT * from [{tableName}] WHERE Id > {position} order by Id";

                var command = new SqlCommand(commandText, connection);

                connection.Open();

                var sqlDataReader = command.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    var eventType = sqlDataReader["EventType"].ToString();
                    var eventBody = sqlDataReader["Body"].ToString();
                    var eventId = sqlDataReader["EventId"].ToString();
                    var id = Convert.ToInt64(sqlDataReader["Id"]);
                    var publishDateTime = DateTime.Parse(sqlDataReader["PublishDateTime"].ToString() ?? string.Empty);
                    var aggregateType = sqlDataReader["AggregateType"].ToString();
                    items.Add(new EventItem()
                    {
                        Body = eventBody,
                        EventId = eventId,
                        EventType = eventType,
                        Id = id,
                        PublishDateTime = publishDateTime,
                        AggregateType = aggregateType
                    });
                }

                connection.Close();
            }

            return items;
        }
    }
}
