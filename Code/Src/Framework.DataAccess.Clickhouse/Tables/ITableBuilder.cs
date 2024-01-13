using ClickHouse.Net.Entities;

namespace Framework.DataAccess.ClickHouse.Tables;

public interface ITableBuilder
{
    Table Build();
}