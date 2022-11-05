using System.Reflection;

namespace Framework.Hazelcast
{
    public class HazelcastConfiguration
    {
        public string ClientName { get; set; }
        public string ClusterName { get; set; }
        public string NetworkingAddresses { get; set; }
        public Assembly Assembly { get; set; }
    }
}
