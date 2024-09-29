namespace Framework.DataAccess.ClickHouse.Migrator.Initial;

public interface ITableBuilder
{
    IEngineBuilder SetTableName(string name);

}