using Framework.DataAccess.CH.Engines;

namespace Framework.DataAccess.CH.Tables;

public interface IEngineBuilder
{
    IColumnBuilder UseMergeTreeEngine();
    IColumnBuilder UseKafkaEngine(KafkaEngineSetting kafkaEngineSetting);
}