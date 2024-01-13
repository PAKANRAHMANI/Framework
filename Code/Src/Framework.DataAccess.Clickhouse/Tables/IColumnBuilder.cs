using Framework.DataAccess.ClickHouse.Columns;

namespace Framework.DataAccess.ClickHouse.Tables;

public interface IColumnBuilder
{
    ITableBuilder WithColumns(List<ClickHouseColumn> columns);
}