namespace Framework.DataAccess.ClickHouse.Migrator.Migrations;

public interface IMigration
{
    void CreateTable();
    void DropTable(string tableName, string clusterName);
}