using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.EventProcessor.DataStore.MongoDB
{
    public class MongoStoreConfig
    {
        public string CursorDocument { get; set; }
        public string EventsDocument { get; set; }
        public int PullingInterval { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
