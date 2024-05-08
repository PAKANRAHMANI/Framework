using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Testing.Core.Persist.MongoDb
{
    public class MongoDbPersistConfiguration
    {
        public string DbName { get; set; }
        public string ConnectionString { get; set; } = "mongodb://127.0.0.1:27017";
        public bool IsPluralCollectionName { get; set; } = false;
        public bool IsUsingTransaction { get; set; } = false;

    }
}
