using Framework.Core.Events;
using Framework.EventProcessor.Events.MassTransit;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.EventProcessor.Events
{
    public interface IEventPublisher
    {
        Task Publish<T>(T @event) where T : IEvent;
    }
}
