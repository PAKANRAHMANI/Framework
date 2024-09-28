namespace Framework.DataAccess.ClickHouse.Migrator.Initial;

public interface IClusterBuilder
{
    IDatabaseBuilder SetClusterName(string clusterName);
}