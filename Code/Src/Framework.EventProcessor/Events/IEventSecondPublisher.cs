using Framework.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.EventProcessor.Events
{
    public interface IEventSecondPublisher
    {
        Task Publish<T>(T @event) where T : IEvent;
    }
}
