using Framework.DataAccess.ClickHouse.Migrator.KafkaEngine;
using Framework.DataAccess.ClickHouse.Migrator.Tables;

namespace Framework.DataAccess.ClickHouse.Migrator;

public abstract class KafkaMergeTreeTemplate()
{
    public void Create(KafkaEngineSetting setting)
    {
        //1-CREATE TABLE With Kafka Engine
        //2-Create Table ReplicatedMergeTree or ReplicatedReplacingMergeTree
        //3-CREATE TABLE Distributed
        //4-CREATE MATERIALIZED VIEW



    }
    //get table columns 
    protected abstract MergeTreeTable GetColumns();
}