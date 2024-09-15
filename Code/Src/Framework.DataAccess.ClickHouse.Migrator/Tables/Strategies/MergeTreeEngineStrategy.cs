using Framework.DataAccess.ClickHouse.Migrator.MergeTreeEngine;
using Framework.DataAccess.ClickHouse.Migrator.Tables.Constructors;

namespace Framework.DataAccess.ClickHouse.Migrator.Tables.Strategies;

public class MergeTreeEngineStrategy(ITableNodeCreator tableNode, MergeTreeEngineType engineType) : TableEngineStrategy(tableNode)
{
    public override void CreateTable(string tableName)
    {

    }

    public override void DropTable(string tableName, string clusterName)
    {
        TableNode.DropTable(tableName, clusterName);
    }
}