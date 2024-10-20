using Framework.DataAccess.ClickHouse.Migrator.Executors;
using Framework.DataAccess.ClickHouse.Migrator.Tables;

namespace Framework.DataAccess.ClickHouse.Migrator.Migrations;

internal class DistributedTableMigration(DistributedTable table, string replicatedTableName, ClickHouseConfiguration clickHouseConfiguration) : IMigration
{
    public string CreateTable()
    {
        var command = @$"CREATE TABLE IF NOT EXISTS {table.TableName} ON CLUSTER {table.ClusterName} AS {table.DatabaseName}.{replicatedTableName} 
                  ENGINE = Distributed ({table.ClusterName}, {table.DatabaseName}, {replicatedTableName}, murmurHash3_64({table.HashColumnName}))";

        command = command.Replace("\n", " ");

        return command;
    }

    public async Task DropTable(string tableName, string clusterName)
    {
        var command = $"DROP TABLE IF EXISTS {tableName} ON CLUSTER {clusterName};";

        await ExecuteCommand.Execute(clickHouseConfiguration, command);
    }
}