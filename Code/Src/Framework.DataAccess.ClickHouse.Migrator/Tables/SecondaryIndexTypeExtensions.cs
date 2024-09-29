namespace Framework.DataAccess.ClickHouse.Migrator.Tables
{
    public static class SecondaryIndexTypeExtensions
    {
        public static string GetValue(this SecondaryIndexType secondaryIndexType)
        {
            return secondaryIndexType switch
            {
                SecondaryIndexType.BloomFilter => "bloom_filter",
                SecondaryIndexType.MinMax => "minmax",
                SecondaryIndexType.NgramBfV1 => "ngrambf_v1",
                SecondaryIndexType.Set => "set",
                SecondaryIndexType.TokenBfV1 => "tokenbf_v1",
                _ => throw new ArgumentOutOfRangeException(nameof(SecondaryIndexType), secondaryIndexType, "SecondaryIndexType is not valid")
            };
        }
    }
}
