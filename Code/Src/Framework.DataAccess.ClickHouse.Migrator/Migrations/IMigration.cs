namespace Framework.DataAccess.ClickHouse.Migrator.Migrations;

public interface IMigration
{
    string CreateTable();
    Task DropTable(string tableName, string clusterName);
}