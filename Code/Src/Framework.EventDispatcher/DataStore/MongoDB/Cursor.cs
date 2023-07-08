using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.EventProcessor.DataStore.MongoDB
{
    public class Cursor
    {
        public long Id { get; set; }
        public long Position { get; set; }
    }
}
