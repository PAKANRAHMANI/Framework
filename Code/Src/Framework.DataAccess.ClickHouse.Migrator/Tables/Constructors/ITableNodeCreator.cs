namespace Framework.DataAccess.ClickHouse.Migrator.Tables.Constructors;

public interface ITableNodeCreator
{
    void CreateTable();
    void DropTable(string tableName, string clusterName);
}