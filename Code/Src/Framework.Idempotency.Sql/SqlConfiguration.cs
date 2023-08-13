using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Idempotency.Sql
{
    public class SqlConfiguration
    {
        public string ConnectionString { get; set; }
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string ReceivedDate { get; set; }
    }
}
