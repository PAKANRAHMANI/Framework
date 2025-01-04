using MongoDB.Driver;

namespace Framework.DataAccess.Mongo
{
    public class MongoClientConfiguration
    {
        /// <summary>
        /// The greatest number of connections a driver's connection pool may be establishing concurrently.
        /// Data Type: integer
        /// Default: 2
        /// </summary>
        public int MaxConnecting { get; set; }
        /// <summary>
        /// The greatest number of clients or connections the driver can create in its
        /// connection pool.This count includes connections in use.
        /// Data Type: integer
        /// Default: 100
        /// </summary>
        public int MaxConnectionPoolSize { get; set; }
        /// <summary>
        /// The readConcern option allows you to control the consistency and isolation properties of the data read from replica sets and sharded clusters.
        /// Level:
        /// 1-local =>
        /// 2-available
        /// 3-majority
        /// 4-linearizable
        /// 5-snapshot
        /// </summary>
        public ReadConcernLevel ReadConcern { get; set; }
        /// <summary>
        /// Read preference describes how MongoDB clients route read operations to the members of a replica set.
        /// By default, an application directs its read operations to the primary member in a replica set (i.e. read preference mode "primary"). But, clients can specify a read preference to send read operations to secondaries.
        /// Read Preference Mode :
        /// 1-primary => 0
        /// 2-primaryPreferred => 1
        /// 3-secondary=> 2
        /// 4-secondaryPreferred => 3
        /// 5-nearest => 4
        /// </summary>
        public ReadPreferenceMode? ReadPreference { get; set; }
        /// <summary>
        /// Replica set members can lag behind the primary due to network congestion, low disk throughput, long-running operations, etc. The read preference maxStalenessSeconds option lets you specify a maximum replication lag, or "staleness", for reads from secondaries. When a secondary's estimated staleness exceeds maxStalenessSeconds, the client stops using it for read operations.
        /// </summary>
        public double MaxStaleness { get; set; }
        /// <summary>
        /// Enables retryable reads.
        /// Data Type: boolean
        /// Default: true
        /// </summary>
        public bool RetryReads { get; set; } = true;

        /// <summary>
        /// Enables retryable writes.
        /// Data Type: boolean
        /// Default: true
        /// </summary>
        public bool RetryWrites { get; set; } = true;
        /// <summary>
        /// Specifies whether to force dispatch all operations to the host.
        /// This property must be set to false if you specify more than one host name.
        /// Data Type: boolean
        /// Default: false
        /// </summary>
        public bool DirectConnection { get; set; } = false;
        /// <summary>
        /// The length of time the driver tries to establish a single TCP socket connection to the server before timing out.
        /// DataType: TimeSpan
        /// Default: 30 seconds
        /// </summary>
        public double ConnectTimeout { get; set; }
        /// <summary>
        /// The number of connections the driver should create and keep in the connection
        /// pool even when no operations are occurring.This count includes connections in use.
        /// Data Type: integer
        /// Default: 0
        /// </summary>
        public int MinConnectionPoolSize { get; set; }
        /// <summary>
        /// The length of time the driver tries to send or receive on a socket before timing out.
        /// Data Type: TimeSpan
        /// Default: OS default
        /// </summary>
        public double SocketTimeout { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
