namespace Framework.DataAccess.ClickHouse.Tables;

public interface IMetadataTableBuilder
{
    IEngineBuilder WithTableName(string name);
}