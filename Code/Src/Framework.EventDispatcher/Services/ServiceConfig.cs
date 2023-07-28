using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.EventProcessor.Services
{
    public class ServiceConfig
    {
        public bool EnableSecondarySending { get; set; } 
        public bool SendWithKafka { get; set; } = false;
        public bool SendWithMassTransit { get; set; } = false;
    }
}
