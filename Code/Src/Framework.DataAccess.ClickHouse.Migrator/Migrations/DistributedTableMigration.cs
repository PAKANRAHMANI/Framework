using Framework.DataAccess.ClickHouse.Migrator.Executors;
using Framework.DataAccess.ClickHouse.Migrator.Tables;

namespace Framework.DataAccess.ClickHouse.Migrator.Migrations;

internal class DistributedTableMigration(DistributedTable table,string replicatedTableName, ClickHouseConfiguration clickHouseConfiguration) : IMigration
{
    public void CreateTable()
    {
        var command = @$"CREATE TABLE {table.TableName} ON CLUSTER {table.ClusterName} AS {table.DatabaseName}.{replicatedTableName} 
                  ENGINE = Distributed ({table.ClusterName}, {table.DatabaseName}, {replicatedTableName}, murmurHash3_64({table.HashColumnName}))";

        ExecuteCommand.Execute(clickHouseConfiguration, command);
    }

    public void DropTable(string tableName, string clusterName)
    {
        var command = $"DROP TABLE IF EXISTS {tableName} ON CLUSTER {clusterName};";

        ExecuteCommand.Execute(clickHouseConfiguration, command);
    }
}