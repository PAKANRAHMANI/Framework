namespace Framework.DataAccess.ClickHouse.Migrator.Tables.Strategies
{
    public interface ITableEngineStrategy
    {
        void CreateTable(string tableName);
        void DropTable(string tableName, string clusterName);
    }
}
