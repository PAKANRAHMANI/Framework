using Framework.DataAccess.CH.Tables;

namespace Framework.DataAccess.CH.Engines;

public class KafkaEngine
{
    public Table KafkaTable { get; init; }
    public Table MergeTreeTable { get; init; }
    public string MaterializeViewName { get; init; }
    public string MaterializeViewSelect { get; init; }
}