using FluentAssertions;

namespace Framework.DataAccess.ClickHouse.Migrator.Tests.Integration
{
    public class KafkaDistributedTemplateTests
    {
        [Fact]
        public void create_replicated_replacing_tables_correctly()
        {
            const string distributedHashColumn = "InstrumentId";

            var createTables = () => new CharacteristicDistributedTable().Create(distributedHashColumn);

            createTables.Should().NotThrow();
        }
    }
}