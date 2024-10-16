namespace Framework.DataAccess.ClickHouse.Migrator.Migrations;

public interface IMigration
{
    Task CreateTable();
    Task DropTable(string tableName, string clusterName);
}