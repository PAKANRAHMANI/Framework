﻿using ClickHouse.Net;

namespace Framework.DataAccess.ClickHouse.Engines;

public abstract class KafkaEngineDistributed(IClickHouseDatabase database)
{
    public void Create(KafkaEngineSetting setting)
    {
        //1-CREATE TABLE With Kafka Engine
        //2-Create Table ReplicatedMergeTree or ReplicatedReplacingMergeTree
        //3-CREATE TABLE Distributed
        //4-CREATE MATERIALIZED VIEW



    }
    //get table columns 
    protected abstract ClickHouseMergeTreeTable GetColumns();
}