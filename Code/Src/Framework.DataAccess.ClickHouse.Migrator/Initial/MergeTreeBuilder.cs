using Framework.DataAccess.ClickHouse.Migrator.MergeTreeEngine;
using Framework.DataAccess.ClickHouse.Migrator.Partitions;
using Framework.DataAccess.ClickHouse.Migrator.Tables;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.DataAccess.ClickHouse.Migrator.Initial;

public sealed class MergeTreeBuilder :
    IClusterBuilder,
    IDatabaseBuilder,
    ITableBuilder,
    IEngineBuilder,
    IMergeTreeBuilder,
    IReplacingMergeTreeBuilder,
    IPartitionBuilder,
    ISecondaryIndexBuilder,
    IMergeTreeFamilyBuilder,
    ITtlBuilder

{
    private string _databaseName;
    private string _tableName;
    private string _clusterName;
    private MergeTreeEngineType _mergeTreeEngineType;
    private bool _usePartition;
    private bool _useTtl;
    private bool _useSecondaryIndex;
    private bool _isReplicated;
    private string _versionColumn;
    private Ttl _ttl;
    private readonly List<SecondaryIndex> _secondaryIndices = [];
    private readonly List<PartitionColumn> _partitionColumns = [];
    private readonly List<string> _primaryKeyColumns = [];
    private readonly List<Setting> _settings = [];
    private MergeTreeBuilder() { }
    public static IClusterBuilder Setup() => new MergeTreeBuilder();

    public MergeTreeTable Build()
    {
        return new MergeTreeTable(_databaseName, _tableName, _clusterName)
        {
            MergeTreeEngineType = _mergeTreeEngineType,
            UsePartition = _usePartition,
            UseTtl = _useTtl,
            UseSecondaryIndex = _useSecondaryIndex,
            IsReplicated = _isReplicated,
            VersionColumn = _versionColumn,
            Ttl = _ttl,
            SecondaryIndices = _secondaryIndices,
            PartitionColumns = _partitionColumns,
            PrimaryKeyColumns = _primaryKeyColumns,
            Settings = _settings,
            DatabaseName = _databaseName,
            TableName = _tableName,
            ClusterName = _clusterName
        };
    }

    public IDatabaseBuilder SetClusterName(string clusterName)
    {
        _clusterName = clusterName;
        return this;
    }

    public ITableBuilder SetDatabaseName(string databaseName)
    {
        _databaseName = databaseName;
        return this;
    }

    public IEngineBuilder SetTableName(string name)
    {
        _tableName = name;
        return this; ;
    }

    public IMergeTreeBuilder AsMergeTree()
    {
        return this;
    }

    public IMergeTreeBuilder AsReplicatedMergeTree()
    {
        _isReplicated = true;
        return this; 
    }

    public IReplacingMergeTreeBuilder AsReplacingMergeTree()
    {
        return this;
    }

    public IReplacingMergeTreeBuilder AsReplicatedReplacingMergeTree()
    {
        _isReplicated = true;
        return this;
    }

    public IMergeTreeBuilder WithMergeTreeEngineType(MergeTreeEngineType engineType)
    {
        _mergeTreeEngineType = engineType;
        return this;
    }

    public IPartitionBuilder HasPartition()
    {
        _usePartition = true;
        return this;
    }

    public ITtlBuilder UseTtl()
    {
        _useTtl = true;
        return this;
    }

    public ISecondaryIndexBuilder HasSecondaryIndex()
    {
        _useSecondaryIndex = true;
        return this;
    }

    public IMergeTreeBuilder WithPrimaryKeyColumns(params string[] primaryKeyColumns)
    {
        _primaryKeyColumns.AddRange(primaryKeyColumns);
        return this;
    }

    public IMergeTreeFamilyBuilder WithSettings(params Setting[] settings)
    {
        _settings.AddRange(settings);
        return this;
    }

    public IMergeTreeBuilder WithPartitionColumns(params PartitionColumn[] partitionColumns)
    {
        _partitionColumns.AddRange(partitionColumns);
        return this;
    }

    public IMergeTreeBuilder WithVersionColumn(string versionColumn)
    {
        _versionColumn = versionColumn;
        return this;
    }

    public IMergeTreeBuilder WithSecondaryIndices(params SecondaryIndex[] indices)
    {
        _secondaryIndices.AddRange(indices);
        return this;
    }

    public IMergeTreeBuilder WithTtl(Ttl ttl)
    {
        _ttl = ttl;
        return this;
    }
}