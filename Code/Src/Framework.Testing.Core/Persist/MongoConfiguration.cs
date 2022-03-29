using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Testing.Core.Persist
{
    public class MongoConfiguration
    {
        public string ConnectionString { get; set; }
        public string DbName { get; set; }
    }
}
