using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Framework.Messages
{
	public static class BaseEventTypes
	{
		public static IReadOnlyCollection<string> TypeNames { get; }
		static BaseEventTypes()
		{
			TypeNames = new ReadOnlyCollection<string>(new List<string>
			{
				$"urn:message:{typeof(Message).Namespace}:{nameof(Message)}",
				$"urn:message:{typeof(IMessage).Namespace}:{nameof(IMessage)}"
			});
		}
	}
}
