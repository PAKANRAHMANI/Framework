using FluentAssertions;

namespace Framework.DataAccess.ClickHouse.Migrator.Tests.Unit
{
    public class KafkaDistributedTemplateTests
    {
        [Fact]
        public void create_replicated_replacing_tables_without_exception()
        {
            const string distributedHashColumn = "InstrumentId";

            var createTables = () => new CharacteristicTable().Create(distributedHashColumn);

            createTables.Should().NotThrow();
        }
    }
}