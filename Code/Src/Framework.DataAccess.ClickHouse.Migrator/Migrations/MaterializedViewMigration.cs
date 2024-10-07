using Framework.DataAccess.ClickHouse.Migrator.Executors;
using Framework.DataAccess.ClickHouse.Migrator.Tables;

namespace Framework.DataAccess.ClickHouse.Migrator.Migrations;

internal class MaterializedViewMigration(Table table, ClickHouseConfiguration clickHouseConfiguration) : IMigration
{
    public void CreateTable()
    {
        var command = @$"CREATE MATERIALIZED VIEW IF NOT EXISTS {table.DatabaseName}.{table.TableName}_MaterializedView
                         ON CLUSTER {table.ClusterName}
                         TO {table.DatabaseName}.{table.TableName}_Distributed AS
                         SELECT *
                         FROM {table.DatabaseName}.{table.TableName}_Messages";

        ExecuteCommand.Execute(clickHouseConfiguration, command);
    }

    public void DropTable(string tableName, string clusterName)
    {
        var command = $"DROP VIEW IF EXISTS {table.DatabaseName}.{table.TableName}_MaterializedView ON CLUSTER {clusterName};";

        ExecuteCommand.Execute(clickHouseConfiguration, command);
    }
}