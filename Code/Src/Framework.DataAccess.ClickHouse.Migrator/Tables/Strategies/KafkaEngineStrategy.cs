using Framework.DataAccess.ClickHouse.Migrator.Tables.Constructors;

namespace Framework.DataAccess.ClickHouse.Migrator.Tables.Strategies;

public class KafkaEngineStrategy(ITableNodeCreator tableNode) : TableEngineStrategy(tableNode)
{
    public override void CreateTable(string tableName)
    {
        TableNode.CreateTable();
    }

    public override void DropTable(string tableName,string clusterName)
    {
        TableNode.DropTable(tableName, clusterName);
    }
}