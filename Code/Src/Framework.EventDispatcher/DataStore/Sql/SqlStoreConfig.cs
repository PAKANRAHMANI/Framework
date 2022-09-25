using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.EventProcessor.DataStore.Sql
{
    public class SqlStoreConfig
    {
        public string CursorTable { get; set; }
        public string EventTable { get; set; }
        public int PullingInterval { get; set; }
        public string ConnectionString { get; set; }
    }
}
