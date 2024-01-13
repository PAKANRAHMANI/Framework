using System.Text;
using ClickHouse.Net.Entities;
using Framework.DataAccess.ClickHouse.Columns;
using Framework.DataAccess.ClickHouse.Engines;

namespace Framework.DataAccess.ClickHouse.Tables;

public class TableBuilder :
    IMetadataTableBuilder,
    IEngineBuilder,
    IColumnBuilder,
    ITableBuilder
{
    private string _tableName;
    private TableIntegrationEngines _engine;
    private List<ClickHouseColumn> _columns = new();
    private KafkaEngineSetting _kafkaEngineSetting;
    private TableBuilder() { }

    public static IMetadataTableBuilder Setup() => new TableBuilder();
    public IEngineBuilder WithTableName(string name)
    {
        _tableName = name;

        return this;
    }

    public IColumnBuilder UseMergeTreeEngine()
    {
        _engine = TableIntegrationEngines.MergeTree;

        return this;
    }

    public IColumnBuilder UseKafkaEngine(KafkaEngineSetting kafkaEngineSetting)
    {
        _engine = TableIntegrationEngines.Kafka;

        _kafkaEngineSetting = kafkaEngineSetting;

        return this;
    }

    public ITableBuilder WithColumns(List<ClickHouseColumn> columns)
    {
        _columns = columns;

        return this;
    }

    public Table Build()
    {
        string engine;
        //TODO:Refactor Switch
        switch (_engine)
        {
            case TableIntegrationEngines.Kafka:

                var kafkaEngine = new StringBuilder();

                kafkaEngine.Append($"{TableIntegrationEngines.Kafka.ToString()}() ");
                kafkaEngine.Append("SETTINGS ");
                kafkaEngine.Append($"kafka_broker_list = '{_kafkaEngineSetting.BootstrapServers}',");
                kafkaEngine.Append($"kafka_topic_list = '{_kafkaEngineSetting.Topics}',");
                kafkaEngine.Append($"kafka_group_name = '{_kafkaEngineSetting.ConsumerGroupId}',");
                kafkaEngine.Append($"kafka_format = '{_kafkaEngineSetting.Format}',");
                kafkaEngine.Append($"kafka_num_consumers = {_kafkaEngineSetting.ConsumerCount},");
                kafkaEngine.Append($"kafka_thread_per_consumer = {_kafkaEngineSetting.ThreadPerConsumerCount},");
                kafkaEngine.Append($"kafka_max_block_size = {_kafkaEngineSetting.MaxBlockSize},");
                kafkaEngine.Append($"kafka_flush_interval_ms = {_kafkaEngineSetting.FlushIntervalMs},");
                kafkaEngine.Append($"kafka_poll_max_batch_size = {_kafkaEngineSetting.PollMaxBatchSize}");

                engine = kafkaEngine.ToString();

                break;

            case TableIntegrationEngines.MergeTree:

                engine = $"{TableIntegrationEngines.MergeTree.ToString()}() ORDER BY tuple()";

                break;

            default:
                throw new ArgumentOutOfRangeException("ClickHouseEngine");
        }

        return new Table
        {
            Name = _tableName,
            Engine = engine,
            Columns = _columns.Map()
        };
    }
}