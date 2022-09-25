using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Core.Events;

namespace Framework.EventProcessor.Filtering
{
    public interface IEventFilter
    {
        bool ShouldPublish(IEvent @event);
    }
}
