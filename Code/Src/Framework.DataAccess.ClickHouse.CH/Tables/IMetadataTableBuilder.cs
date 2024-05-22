namespace Framework.DataAccess.CH.Tables;

public interface IMetadataTableBuilder
{
    IEngineBuilder WithTableName(string name);
}