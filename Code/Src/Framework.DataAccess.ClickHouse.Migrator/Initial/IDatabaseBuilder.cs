namespace Framework.DataAccess.ClickHouse.Migrator.Initial;

public interface IDatabaseBuilder
{
    ITableBuilder SetDatabaseName(string databaseName);

}