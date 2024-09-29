using Framework.DataAccess.ClickHouse.Migrator.Columns;
using Framework.DataAccess.ClickHouse.Migrator.Initial;
using Framework.DataAccess.ClickHouse.Migrator.KafkaEngine;
using Framework.DataAccess.ClickHouse.Migrator.MergeTreeEngine;
using Framework.DataAccess.ClickHouse.Migrator.Tables;

namespace Framework.DataAccess.ClickHouse.Migrator.Tests.Integration;

public class CharacteristicDistributedTable : KafkaDistributedTemplate
{
    public string InstrumentId { get; set; }
    public bool IsOption { get; set; }
    public string CompanyCode { get; set; }
    public string InstrumentType { get; set; }
    public string MarketPlace { get; set; }
    public string BoardName { get; set; }
    public double LowestAllowedVolume { get; set; }
    public double HighestAllowedVolume { get; set; }
    public long TickQuantity { get; set; }
    public string GroupCode { get; set; }
    public decimal YesterdayClosingPrice { get; set; }
    public decimal TickPrice { get; set; }
    public string AssetClassTitle { get; set; }
    public short BoardCode { get; set; }
    public string IndustryCode { get; set; }
    public string IndustryTitle { get; set; }
    public string IndustryColor { get; set; }
    public string SubIndustryCode { get; set; }
    public string AssetClassColor { get; set; }
    public string SubIndustryTitle { get; set; }
    public long NumberOfSharesOrBondsOutstanding { get; set; }
    public long MessageOffset { get; set; }
    public string RlcCreationTime { get; set; }
    public string RlcCreationDate { get; set; }
    public DateTime CreationDateTime { get; set; }
    public DateTime CreateDateTime { get; set; }
    protected override List<Column> GetColumns()
    {
        var kafkaTableColumns = new ColumnBuilder()
            .WithName(nameof(CharacteristicDistributedTable.InstrumentId)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicDistributedTable.IsOption)).WithType(ColumnDataTypes.Bool)
            .WithName(nameof(CharacteristicDistributedTable.CompanyCode)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicDistributedTable.InstrumentType)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicDistributedTable.MarketPlace)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicDistributedTable.BoardName)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicDistributedTable.LowestAllowedVolume)).WithType(ColumnDataTypes.Float64)
            .WithName(nameof(CharacteristicDistributedTable.HighestAllowedVolume)).WithType(ColumnDataTypes.Float64)
            .WithName(nameof(CharacteristicDistributedTable.TickQuantity)).WithType(ColumnDataTypes.Int64)
            .WithName(nameof(CharacteristicDistributedTable.GroupCode)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicDistributedTable.YesterdayClosingPrice)).WithType(ColumnDataTypes.Float64)
            .WithName(nameof(CharacteristicDistributedTable.TickPrice)).WithType(ColumnDataTypes.Float64)
            .WithName(nameof(CharacteristicDistributedTable.AssetClassTitle)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicDistributedTable.BoardCode)).WithType(ColumnDataTypes.Int16)
            .WithName(nameof(CharacteristicDistributedTable.IndustryCode)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicDistributedTable.IndustryTitle)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicDistributedTable.IndustryColor)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicDistributedTable.SubIndustryCode)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicDistributedTable.AssetClassColor)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicDistributedTable.SubIndustryTitle)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicDistributedTable.NumberOfSharesOrBondsOutstanding)).WithType(ColumnDataTypes.Int64)
            .WithName(nameof(CharacteristicDistributedTable.MessageOffset)).WithType(ColumnDataTypes.Int64)
            .WithName(nameof(CharacteristicDistributedTable.RlcCreationDate)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicDistributedTable.RlcCreationTime)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicDistributedTable.CreationDateTime)).WithType(ColumnDataTypes.DateTime64)
            .WithName(nameof(CharacteristicDistributedTable.CreateDateTime)).WithType(ColumnDataTypes.DateTime)
            .Build();

        return kafkaTableColumns;
    }

    protected override MergeTreeTable GetMergeTreeTable()
    {
        return MergeTreeBuilder
            .Setup()
            .SetClusterName(ClickHouseTestData.ChsCip)
            .SetDatabaseName(ClickHouseTestData.RlcMessages)
            .SetTableName(ClickHouseTestData.Characteristics)
            .AsReplicatedReplacingMergeTree()
            .WithVersionColumn(ClickHouseTestData.CreationDateTime)
            .UseTtl()
            .WithTtl(ClickHouseTestData.Day)
            .WithMergeTreeEngineType(MergeTreeEngineType.ReplicatedReplacingMergeTree)
            .WithPrimaryKeyColumns(ClickHouseTestData.InstrumentId)
            .HasSecondaryIndex()
            .WithSecondaryIndices(ClickHouseTestData.MinMax)
            .WithSettings(ClickHouseTestData.IndexGranularity)
            .Build();
    }

    protected override KafkaEngineSetting GetKafkaEngineSetting()
    {
        return new KafkaEngineSettingBuilder()
            .WithBootstrapServers(ClickHouseTestData.CipTraderCharismaTech9092)
            .WithTopics(ClickHouseTestData.CharacteristicsTest2)
            .WithConsumerGroupId(ClickHouseTestData.ClickhouseCharacteristicsGroupIdTest2)
            .WithFlushIntervalMs(ClickHouseTestData._10)
            .Build();
    }

    protected override ClickHouseConfiguration GetClickHouseConfiguration()
    {
        return new ClickHouseConfigurationBuilder()
            .WithHost(ClickHouseTestData._10_104_142_30)
            .WithPort(ClickHouseTestData._30536)
            .WithUsername(ClickHouseTestData.Admin)
            .WithPassword(ClickHouseTestData.Password)
            .WithDatabaseName(ClickHouseTestData.RlcMessages)
            .WithCommandTimeout(ClickHouseTestData._5000)
            .WithReadWriteTimeout(ClickHouseTestData._6000)
            .Build();
    }
}