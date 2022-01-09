using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Core.Events;

namespace Framework.Core
{
    public class DomainEventPersistenceBuilder: IDomainEventPersistenceBuilder
    {
        private readonly Dictionary<string, Func<IDomainEvent, object>> _columns;
        private string _tableName;

        public DomainEventPersistenceBuilder()
        {
            _columns = new Dictionary<string, Func<IDomainEvent, object>>();
        }
        public IDomainEventPersistenceBuilder WithTableName(string name)
        {
            _tableName = name;
            return this;
        }

        public IDomainEventPersistenceBuilder WithColumn(string columnName, Func<IDomainEvent, object> valueProvider)
        {
            _columns.Add(columnName, valueProvider);
            return this;
        }
        public Dictionary<string, Func<IDomainEvent, object>> GetColumns()
        {
            return _columns;
        }
        public string Build()
        {
            var columnNames = _columns.Select(a => a.Key).ToList();
            return $"INSERT INTO [{_tableName}]({string.Join(", ", columnNames)}) VALUES({string.Join(", ", columnNames.Select(column=>$"@{column.ToLower()}"))})";
        }
    }
}
