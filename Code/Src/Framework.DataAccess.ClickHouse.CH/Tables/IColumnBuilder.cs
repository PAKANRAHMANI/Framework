using Framework.DataAccess.CH.Columns;

namespace Framework.DataAccess.CH.Tables;

public interface IColumnBuilder
{
    ITableBuilder WithColumns(List<ClickHouseColumn> columns);
}