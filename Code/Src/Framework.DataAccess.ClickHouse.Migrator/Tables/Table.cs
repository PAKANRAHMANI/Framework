namespace Framework.DataAccess.ClickHouse.Migrator.Tables
{
    public class Table(string databaseName, string tableName, string clusterName)
    {
        public string DatabaseName { get; init; } = databaseName;
        public string TableName { get; init; } = tableName;
        public string ClusterName { get; init; } = clusterName;
    }
}
