using System.Collections.ObjectModel;
using Framework.Core.Events;

namespace Framework.MassTransit.Message
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
