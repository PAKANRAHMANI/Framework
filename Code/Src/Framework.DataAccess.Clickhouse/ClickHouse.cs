using ClickHouse.Net;
using ClickHouse.Net.Entities;
using Framework.DataAccess.ClickHouse.Views;
using System.Text;

namespace Framework.DataAccess.ClickHouse;

internal sealed class ClickHouse : IClickHouse
{
    private readonly IClickHouseDatabase _database;

    public ClickHouse(IClickHouseDatabase database)
    {
        _database = database;
    }

    public void CreateTable(Table table)
    {
        try
        {

            _database.CreateTable(table, new CreateOptions { IfNotExists = true });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void CreateMaterializeView(MaterializeView materializeView)
    {
        var command = new StringBuilder();

        command.Append("CREATE MATERIALIZED VIEW IF NOT EXISTS ");
        command.Append($"{materializeView.Name} TO ");
        command.Append($"{materializeView.DestinationTable} AS ");
        command.Append($"{materializeView.Select} FROM ");
        command.Append($"{materializeView.SourceTable}");

        _database.ExecuteNonQuery(command.ToString());
    }

    public void DropTable(string tableName)
    {
        _database.DropTable(tableName, new DropOptions { IfExists = true });
    }
}