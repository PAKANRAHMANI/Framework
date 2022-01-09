using System;
using System.Collections.Generic;
using System.Text;
using Framework.Core.Events;

namespace Framework.Core
{
    public interface IDomainEventPersistenceBuilder
    {
        IDomainEventPersistenceBuilder WithTableName(string name);
        IDomainEventPersistenceBuilder WithColumn(string columnName, Func<IDomainEvent, object> valueProvider);
        Dictionary<string, Func<IDomainEvent, object>> GetColumns();
        string Build();
    }
}
