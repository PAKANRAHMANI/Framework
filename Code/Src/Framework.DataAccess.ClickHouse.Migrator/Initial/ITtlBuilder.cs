using Framework.DataAccess.ClickHouse.Migrator.Tables;

namespace Framework.DataAccess.ClickHouse.Migrator.Initial;

public interface ITtlBuilder
{
    IMergeTreeBuilder WithTtl(Ttl ttl);
}