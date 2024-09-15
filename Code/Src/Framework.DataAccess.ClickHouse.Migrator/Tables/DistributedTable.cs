namespace Framework.DataAccess.ClickHouse.Migrator.Tables;

public class DistributedTable(string databaseName, string tableName, string clusterName) :Table(databaseName, tableName, clusterName)
{
    public string HashColumnName { get; init; } 
}