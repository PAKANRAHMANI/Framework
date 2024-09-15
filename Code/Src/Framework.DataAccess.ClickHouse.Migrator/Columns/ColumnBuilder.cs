namespace Framework.DataAccess.ClickHouse.Migrator.Columns;

public class ColumnBuilder
{
    private Column CurrentColumn { get; set; }
    private readonly List<Column> _columns = [];

    public ColumnBuilder WithName(string name)
    {
        var column = new Column { Name = name, Type = ColumnDataTypes.String };
        _columns.Add(column);
        CurrentColumn = column;
        return this;
    }
    public ColumnBuilder WithType(string type)
    {
        CurrentColumn.Type = type;
        return this;
    }

    public List<Column> Build()
    {
        return _columns;
    }
}