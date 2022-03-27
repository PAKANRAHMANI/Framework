using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.MassTransit
{
    public class RetryConfiguration
    {
        public int RetryCount { get; set; }
        public int Interval { get; set; }
    }
}
