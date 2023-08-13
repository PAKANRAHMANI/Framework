using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Idempotency.Mongo
{
    public class MongoConfiguration
    {
        public string CollectionName { get; set; }
        public string FieldName { get; set; }
        public string ReceivedDate { get; set; }
    }
}
