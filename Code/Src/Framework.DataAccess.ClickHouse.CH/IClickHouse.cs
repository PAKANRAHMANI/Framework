using Framework.DataAccess.CH.Tables;
using Framework.DataAccess.CH.Views;

namespace Framework.DataAccess.CH;

public interface IClickHouse
{
    Task CreateTable(Table table);
    Task CreateMaterializeView(MaterializeView materializeView);
    Task DropTable(string database,string tableName);
    Task DropView(string database,string viewName);
}