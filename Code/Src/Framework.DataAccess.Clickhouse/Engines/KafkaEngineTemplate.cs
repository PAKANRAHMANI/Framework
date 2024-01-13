using Framework.DataAccess.ClickHouse.Views;

namespace Framework.DataAccess.ClickHouse.Engines
{
    public class KafkaEngineTemplate : IKafkaEngineTemplate
    {
        private readonly IClickHouse _clickHouse;

        public KafkaEngineTemplate(IClickHouse clickHouse)
        {
            _clickHouse = clickHouse;
        }
        public void Create(KafkaEngine kafkaEngine)
        {
            _clickHouse.CreateTable(kafkaEngine.KafkaTable);

            _clickHouse.CreateTable(kafkaEngine.MergeTreeTable);

            _clickHouse.CreateMaterializeView(MaterializeView.Create(
                kafkaEngine.MaterializeViewName,
                kafkaEngine.MaterializeViewSelect,
                kafkaEngine.KafkaTable.Name,
                kafkaEngine.MergeTreeTable.Name));
        }
    }
}
