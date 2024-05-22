using Framework.DataAccess.CH.Views;

namespace Framework.DataAccess.CH.Engines
{
    public class KafkaEngineTemplate(IClickHouse clickHouse) : IKafkaEngineTemplate
    {
        public async Task Create(KafkaEngine kafkaEngine)
        {
            await clickHouse.CreateTable(kafkaEngine.KafkaTable);

            await clickHouse.CreateTable(kafkaEngine.MergeTreeTable);

            await clickHouse.CreateMaterializeView(MaterializeView.Create(
                 kafkaEngine.MaterializeViewName,
                 kafkaEngine.MaterializeViewSelect,
                 kafkaEngine.KafkaTable.Name,
                 kafkaEngine.MergeTreeTable.Name));
        }
    }
}
