using Framework.DataAccess.ClickHouse.Engines;

namespace Framework.DataAccess.ClickHouse.Tables;

public interface IEngineBuilder
{
    IColumnBuilder UseMergeTreeEngine();
    IColumnBuilder UseKafkaEngine(KafkaEngineSetting kafkaEngineSetting);
}