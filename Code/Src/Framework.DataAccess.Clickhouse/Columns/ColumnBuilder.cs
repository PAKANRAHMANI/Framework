namespace Framework.DataAccess.ClickHouse.Columns;

public class ColumnBuilder
{
    private ClickHouseColumn CurrentColumn { get; set; }
    private readonly List<ClickHouseColumn> _columns = new();

    public ColumnBuilder WithName(string name)
    {
        var column = new ClickHouseColumn { Name = name, Type = ColumnConstant.DefaultType };
        _columns.Add(column);
        CurrentColumn = column;
        return this;
    }
    public ColumnBuilder WithType(string type)
    {
        CurrentColumn.Type = type;
        return this;
    }

    public List<ClickHouseColumn> Build()
    {
        return _columns;
    }
}