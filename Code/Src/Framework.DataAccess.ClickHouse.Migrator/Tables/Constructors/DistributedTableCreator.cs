namespace Framework.DataAccess.ClickHouse.Migrator.Tables.Constructors;

public class DistributedTableCreator(DistributedTable table) : ITableNodeCreator
{
    public void CreateTable()
    {
        var command = @$"CREATE TABLE {table.TableName} ON CLUSTER {table.ClusterName} AS {table.DatabaseName}.{table.TableName} 
                  ENGINE = Distributed ({table.ClusterName}, {table.DatabaseName}, {table.TableName}, murmurHash3_64({table.HashColumnName}))";
    }

    public void DropTable(string tableName, string clusterName)
    {
        var command = $"DROP TABLE IF EXISTS {tableName} ON CLUSTER {clusterName};";
    }
}