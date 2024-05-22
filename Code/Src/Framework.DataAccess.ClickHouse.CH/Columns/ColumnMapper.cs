namespace Framework.DataAccess.CH.Columns
{
    internal static class ColumnMapper
    {
        public static List<Column> Map(this List<ClickHouseColumn> columns)
        {
            return columns.Select(Map).ToList();
        }
        private static Column Map(ClickHouseColumn column)
        {
            return new Column(column.Name, column.Type);
        }
    }
}
