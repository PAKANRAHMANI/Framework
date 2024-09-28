using Framework.DataAccess.ClickHouse.Migrator.Columns;
using Framework.DataAccess.ClickHouse.Migrator.Initial;
using Framework.DataAccess.ClickHouse.Migrator.KafkaEngine;
using Framework.DataAccess.ClickHouse.Migrator.MergeTreeEngine;
using Framework.DataAccess.ClickHouse.Migrator.Tables;

namespace Framework.DataAccess.ClickHouse.Migrator.Tests.Unit;

public class CharacteristicTable : KafkaDistributedTemplate
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
            .WithName(nameof(CharacteristicTable.InstrumentId)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicTable.IsOption)).WithType(ColumnDataTypes.Bool)
            .WithName(nameof(CharacteristicTable.CompanyCode)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicTable.InstrumentType)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicTable.MarketPlace)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicTable.BoardName)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicTable.LowestAllowedVolume)).WithType(ColumnDataTypes.Float64)
            .WithName(nameof(CharacteristicTable.HighestAllowedVolume)).WithType(ColumnDataTypes.Float64)
            .WithName(nameof(CharacteristicTable.TickQuantity)).WithType(ColumnDataTypes.Int64)
            .WithName(nameof(CharacteristicTable.GroupCode)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicTable.YesterdayClosingPrice)).WithType(ColumnDataTypes.Float64)
            .WithName(nameof(CharacteristicTable.TickPrice)).WithType(ColumnDataTypes.Float64)
            .WithName(nameof(CharacteristicTable.AssetClassTitle)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicTable.BoardCode)).WithType(ColumnDataTypes.Int16)
            .WithName(nameof(CharacteristicTable.IndustryCode)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicTable.IndustryTitle)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicTable.IndustryColor)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicTable.SubIndustryCode)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicTable.AssetClassColor)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicTable.SubIndustryTitle)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicTable.NumberOfSharesOrBondsOutstanding)).WithType(ColumnDataTypes.Int64)
            .WithName(nameof(CharacteristicTable.MessageOffset)).WithType(ColumnDataTypes.Int64)
            .WithName(nameof(CharacteristicTable.RlcCreationDate)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicTable.RlcCreationTime)).WithType(ColumnDataTypes.String)
            .WithName(nameof(CharacteristicTable.CreationDateTime)).WithType(ColumnDataTypes.DateTime64)
            .WithName(nameof(CharacteristicTable.CreateDateTime)).WithType(ColumnDataTypes.DateTime)
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