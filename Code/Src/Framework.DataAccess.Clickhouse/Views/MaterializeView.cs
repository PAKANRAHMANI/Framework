namespace Framework.DataAccess.ClickHouse.Views;

public sealed class MaterializeView
{
    public string Name { get; init; }
    public string Select { get; init; }
    public string SourceTable { get; init; }
    public string DestinationTable { get; init; }

    private MaterializeView(string name, string select, string sourceTable, string destinationTable)
    {
        Name = name;
        Select = select;
        SourceTable = sourceTable;
        DestinationTable = destinationTable;
    }

    public static MaterializeView Create(string name, string select, string sourceTable, string destinationTable)
    {
        return new MaterializeView(name, select, sourceTable, destinationTable);
    }
}