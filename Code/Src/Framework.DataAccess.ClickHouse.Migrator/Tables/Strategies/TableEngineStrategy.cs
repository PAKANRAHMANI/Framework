using Framework.DataAccess.ClickHouse.Migrator.Tables.Constructors;

namespace Framework.DataAccess.ClickHouse.Migrator.Tables.Strategies;

public abstract class TableEngineStrategy(ITableNodeCreator tableNode) : ITableEngineStrategy
{
    protected readonly ITableNodeCreator TableNode = tableNode;
    public abstract void CreateTable(string tableName);
    public abstract void DropTable(string tableName,string clusterName);
}