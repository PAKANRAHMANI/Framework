using Framework.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.EventProcessor.DataStore.ChangeTrackers
{
    public interface IReceiverObserver
    {
        void Receive(IEvent @event);
    }
}
