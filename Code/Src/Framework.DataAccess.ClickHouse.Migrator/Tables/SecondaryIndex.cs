﻿namespace Framework.DataAccess.ClickHouse.Migrator.Tables;

public class SecondaryIndex
{
    public string IndexName { get; set; }
    public string ColumnName { get; set; }
    public long Granularity { get; set; }
    public SecondaryIndexType IndexType { get; set; }
    public List<string> IndexParameters { get; set; }
}