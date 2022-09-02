using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Domain;

namespace Framework.Testing.Core.Fakes
{
    public class FakeConfigurator : IAggregateRootConfigurator
    {
        public T Config<T>(T aggregateRoot) where T : IAggregateRoot
        {
            aggregateRoot.SetPublisher(new FakePublisher());

            return aggregateRoot;
        }
    }
}
