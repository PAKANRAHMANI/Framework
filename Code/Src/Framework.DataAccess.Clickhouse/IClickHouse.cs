using ClickHouse.Net.Entities;
using Framework.DataAccess.ClickHouse.Views;

namespace Framework.DataAccess.ClickHouse;

public interface IClickHouse
{
    void CreateTable(Table table);
    void CreateMaterializeView(MaterializeView materializeView);
    void DropTable(string tableName);
}