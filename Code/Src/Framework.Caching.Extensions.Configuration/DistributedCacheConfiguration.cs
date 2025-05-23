﻿namespace Framework.Caching.Extensions.Configuration
{
    public class DistributedCacheConfiguration
    {
        public bool UseFromInstanceNameInKey { get; set; }
        public string Connection { get; set; }
        public string InstanceName { get; set; }
        public int DbNumber { get; set; }
    }
}
