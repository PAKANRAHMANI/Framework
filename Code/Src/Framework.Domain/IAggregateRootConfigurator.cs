using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Domain
{
    public interface IAggregateRootConfigurator
    {
        T Config<T>(T aggregateRoot) where T : IAggregateRoot;
    }
}
