using Framework.DataAccess.ClickHouse.Migrator.Columns;
using Framework.DataAccess.ClickHouse.Migrator.Executors;
using Framework.DataAccess.ClickHouse.Migrator.KafkaEngine;
using Framework.DataAccess.ClickHouse.Migrator.Tables;
using System.Text;

namespace Framework.DataAccess.ClickHouse.Migrator.Migrations
{
    internal class KafkaEngineTableMigration(Table table, List<Column> clickhouseColumns, KafkaEngineSetting kafkaEngineSetting, ClickHouseConfiguration clickHouseConfiguration) : IMigration
    {
        public async Task CreateTable()
        {
            var columnsBuilder = new StringBuilder();

            foreach (var column in clickhouseColumns)
            {
                columnsBuilder.Append($"`{column.Name}` {column.Type},");
            }
            var columns = columnsBuilder.ToString();

            if (columns.EndsWith(","))
                columns = columns.TrimEnd(',');

            var tableBuilder = new StringBuilder();

            tableBuilder.Append($"CREATE TABLE IF NOT EXISTS {table.DatabaseName}.{table.TableName} ON CLUSTER {table.ClusterName}");

            tableBuilder.AppendLine("(");

            tableBuilder.AppendLine($"{columns}");

            tableBuilder.AppendLine(")");

            tableBuilder.AppendLine("ENGINE = Kafka");

            tableBuilder.AppendLine("SETTINGS");

            tableBuilder.AppendLine($"kafka_broker_list = '{kafkaEngineSetting.BootstrapServers}',");

            var topicBuilder = new StringBuilder();

            foreach (var topic in kafkaEngineSetting.Topics)
            {
                topicBuilder.Append($"'{topic}',");
            }
            var topics = topicBuilder.ToString();

            tableBuilder.AppendLine($"kafka_topic_list = {topics}");

            tableBuilder.AppendLine($"kafka_group_name = '{kafkaEngineSetting.ConsumerGroupId}',");

            tableBuilder.AppendLine($"kafka_format = 'JSONEachRow',");

            tableBuilder.AppendLine($"kafka_flush_interval_ms = {kafkaEngineSetting.FlushIntervalMs}");

            var command = tableBuilder.ToString();

            await ExecuteCommand.Execute(clickHouseConfiguration, command);
        }

        public async Task DropTable(string tableName, string clusterName)
        {
            var command = $"DROP TABLE IF EXISTS {tableName} ON CLUSTER {clusterName};";

            await ExecuteCommand.Execute(clickHouseConfiguration, command);
        }
    }
}
