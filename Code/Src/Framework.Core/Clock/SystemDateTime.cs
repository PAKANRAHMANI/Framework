using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Clock
{
    public class SystemDateTime : IDateTime
    {
        public DateTime Now()
        {
            return DateTime.UtcNow;
        }
    }
}
