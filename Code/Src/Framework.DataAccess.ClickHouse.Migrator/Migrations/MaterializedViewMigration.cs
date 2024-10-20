using Framework.DataAccess.ClickHouse.Migrator.Executors;
using Framework.DataAccess.ClickHouse.Migrator.Tables;

namespace Framework.DataAccess.ClickHouse.Migrator.Migrations;

internal class MaterializedViewMigration(Table table, ClickHouseConfiguration clickHouseConfiguration) : IMigration
{
    public string CreateTable()
    {
        var command = @$"CREATE MATERIALIZED VIEW IF NOT EXISTS {table.DatabaseName}.{table.TableName}_MaterializedView
                         ON CLUSTER {table.ClusterName}
                         TO {table.DatabaseName}.{table.TableName}_Distributed AS
                         SELECT *
                         FROM {table.DatabaseName}.{table.TableName}_Messages";

        return command;
    }

    public async Task DropTable(string tableName, string clusterName)
    {
        var command = $"DROP VIEW IF EXISTS {table.DatabaseName}.{table.TableName}_MaterializedView ON CLUSTER {clusterName};";

        await ExecuteCommand.Execute(clickHouseConfiguration, command);
    }
}