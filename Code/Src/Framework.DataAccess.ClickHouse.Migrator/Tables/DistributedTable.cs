namespace Framework.DataAccess.ClickHouse.Migrator.Tables;

internal class DistributedTable(string hashColumnName, string databaseName, string tableName, string clusterName) :Table(databaseName, tableName, clusterName)
{
    public string HashColumnName { get; init; } = hashColumnName;
}