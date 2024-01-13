using ClickHouse.Net.Entities;

namespace Framework.DataAccess.ClickHouse.Engines;

public class KafkaEngineBuilder
{
    private Table _kafkaTable = new();
    private Table _mergeTreeTable = new();
    private string _materializeViewName = string.Empty;
    private string _materializeViewSelect = string.Empty;
    public KafkaEngineBuilder WithKafkaTable(Table table)
    {
        _kafkaTable = table;

        return this;
    }
    public KafkaEngineBuilder WithMergeTreeTable(Table table)
    {
        _mergeTreeTable = table;

        return this;
    }
    public KafkaEngineBuilder WithMaterializeViewName(string name)
    {
        _materializeViewName = name;

        return this;
    }
    public KafkaEngineBuilder WithMaterializeViewSelect(string select)
    {
        _materializeViewSelect = select;

        return this;
    }
    public KafkaEngine Build()
    {
        return new KafkaEngine
        {
            KafkaTable = _kafkaTable,
            MergeTreeTable = _mergeTreeTable,
            MaterializeViewName = _materializeViewName,
            MaterializeViewSelect = _materializeViewSelect
        };
    }
}