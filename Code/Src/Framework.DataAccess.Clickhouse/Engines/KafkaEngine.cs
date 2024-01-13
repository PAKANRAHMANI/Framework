using ClickHouse.Net.Entities;

namespace Framework.DataAccess.ClickHouse.Engines;

public class KafkaEngine
{
    public Table KafkaTable { get; init; }
    public Table MergeTreeTable { get; init; }
    public string MaterializeViewName { get; init; }
    public string MaterializeViewSelect { get; init; }
}