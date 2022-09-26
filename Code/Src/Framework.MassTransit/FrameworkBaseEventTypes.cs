using Framework.Core.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Framework.MassTransit
{
    public static class FrameworkBaseEventTypes
	{
		public static IReadOnlyCollection<string> EventTypeNames { get; }
		static FrameworkBaseEventTypes()
		{
            EventTypeNames = new ReadOnlyCollection<string>(new List<string>
			{
				   $"urn:message:{typeof(IDomainEvent).Namespace}:{nameof(IDomainEvent)}",
				   $"urn:message:{typeof(IEvent).Namespace}:{nameof(IEvent)}",
				   $"urn:message:{typeof(DomainEvent).Namespace}:{nameof(DomainEvent)}",
			});
		}
	}
}
