using FluentAssertions;

namespace Framework.DataAccess.ClickHouse.Migrator.Tests.Integration;

public class KafkaMergeTreeTemplateTests
{
    [Fact]
    public void create_replicated_tables_correctly()
    {
        var createTables = () => new SymbolReplicatedTable().Create();

        createTables.Should().NotThrow();
    }
}