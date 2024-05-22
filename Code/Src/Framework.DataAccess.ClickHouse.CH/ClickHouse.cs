using System.Text;
using System.Xml.Linq;
using Framework.DataAccess.CH.Connections;
using Framework.DataAccess.CH.Tables;
using Framework.DataAccess.CH.Views;
using Octonica.ClickHouseClient;

namespace Framework.DataAccess.CH;


internal sealed class ClickHouse(ConnectionStringBuilder connectionStringBuilder) : IClickHouse
{
    private readonly string _connectionString = connectionStringBuilder.Build();

    public async Task CreateTable(Table table)
    {
        try
        {
            await using var connection = new ClickHouseConnection(this._connectionString);

            connection.Open();

            var tableCreationCommand = CreateCommandToCreateTheTable(table);

            await using var command = connection.CreateCommand(tableCreationCommand);

            await command.ExecuteNonQueryAsync();

            await connection.CloseAsync();
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task CreateMaterializeView(MaterializeView materializeView)
    {
        try
        {
            var materializeViewCreationCommand = CreateCommandToMaterializeView(materializeView);

            await using var connection = new ClickHouseConnection(this._connectionString);

            connection.Open();

            await using var command = connection.CreateCommand(materializeViewCreationCommand);

            await command.ExecuteNonQueryAsync();

            await connection.CloseAsync();
        }
        catch (Exception e)
        {
            throw e;
        }
    }


    public async Task DropTable(string database, string tableName)
    {
        try
        {
            await using var connection = new ClickHouseConnection(this._connectionString);

            connection.Open();

            await using var command = connection.CreateCommand($"DROP TABLE IF EXISTS {database}.{tableName}");

            await command.ExecuteNonQueryAsync();

            await connection.CloseAsync();
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public async Task DropView(string database, string viewName)
    {
        try
        {
            await using var connection = new ClickHouseConnection(this._connectionString);

            connection.Open();

            await using var command = connection.CreateCommand($"DROP VIEW IF EXISTS {database}.{viewName}");

            await command.ExecuteNonQueryAsync();

            await connection.CloseAsync();
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    private static string CreateCommandToCreateTheTable(Table table)
    {
        var commandText = new StringBuilder();

        commandText.Append("CREATE TABLE IF NOT EXISTS ");

        if (string.IsNullOrEmpty(table.Schema))
            commandText.Append($"{table.Name} ");
        else
            commandText.Append($"{table.Schema}.{table.Name} ");

        commandText.Append("(");

        foreach (var column in table.Columns)
        {
            commandText.Append($"{column.Name} ");
            commandText.Append($"{column.Type}, ");
        }

        commandText.Append(") ");

        commandText.Append($"engine {table.Engine}");

        return commandText.ToString();
    }
    private static string CreateCommandToMaterializeView(MaterializeView materializeView)
    {
        var materializeViewCreationCommand = new StringBuilder();

        materializeViewCreationCommand.Append("CREATE MATERIALIZED VIEW IF NOT EXISTS ");
        materializeViewCreationCommand.Append($"{materializeView.Name} TO ");
        materializeViewCreationCommand.Append($"{materializeView.DestinationTable} AS ");
        materializeViewCreationCommand.Append($"{materializeView.Select} FROM ");
        materializeViewCreationCommand.Append($"{materializeView.SourceTable}");
        return materializeViewCreationCommand.ToString();
    }

}