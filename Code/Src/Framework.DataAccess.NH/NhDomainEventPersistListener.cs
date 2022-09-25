using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Event;
using Framework.Core;
using Framework.Core.Events;
using Framework.Domain;

namespace Framework.DataAccess.NH
{
    public class NhDomainEventPersistListener :
          IPreDeleteEventListener,
          IPreInsertEventListener,
          IPreUpdateEventListener
    {
        private readonly IDomainEventPersistenceBuilder _persistenceBuilder;

        public NhDomainEventPersistListener(IDomainEventPersistenceBuilder persistenceBuilder)
        {
            _persistenceBuilder = persistenceBuilder;
        }
        public async Task<bool> OnPreDeleteAsync(PreDeleteEvent @event, CancellationToken cancellationToken)
        {
            return await HandleAsync(@event.Session, @event.Entity);
        }

        public bool OnPreDelete(PreDeleteEvent @event)
        {
            return Handle(@event.Session, @event.Entity);
        }

        public async Task<bool> OnPreInsertAsync(PreInsertEvent @event, CancellationToken cancellationToken)
        {
            return await HandleAsync(@event.Session, @event.Entity);
        }

        public bool OnPreInsert(PreInsertEvent @event)
        {
            return Handle(@event.Session, @event.Entity);
        }

        public async Task<bool> OnPreUpdateAsync(PreUpdateEvent @event, CancellationToken cancellationToken)
        {
            return await HandleAsync(@event.Session, @event.Entity);
        }

        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            return Handle(@event.Session, @event.Entity);
        }
        private bool Handle(ISession session, object entity)
        {
            var aggregateRoot = entity as IAggregateRoot;
            if (aggregateRoot == null) return false;
            foreach (var @event in aggregateRoot.GetEvents())
            {
                var commandText = _persistenceBuilder.Build();
                var command = new SqlCommand(commandText);
                var columns = _persistenceBuilder.GetColumns();
                AddParametersToCommand(command, @event, columns);
                command.Connection = session.Connection as SqlConnection;
                session.GetCurrentTransaction().Enlist(command);
                command.ExecuteNonQuery();
            }
            aggregateRoot.ClearEvents();
            return false;
        }
        private async Task<bool> HandleAsync(ISession session, object entity)
        {
            var aggregateRoot = entity as IAggregateRoot;
            if (aggregateRoot == null) return false;
            foreach (var @event in aggregateRoot.GetEvents())
            {
                var commandText = _persistenceBuilder.Build();
                var command = new SqlCommand(commandText);
                var columns = _persistenceBuilder.GetColumns();
                AddParametersToCommand(command, @event, columns);
                command.Connection = session.Connection as SqlConnection;
                session.GetCurrentTransaction().Enlist(command);
                await command.ExecuteNonQueryAsync();
            }
            aggregateRoot.ClearEvents();
            return false;
        }
        private void AddParametersToCommand(SqlCommand command, IDomainEvent @event, Dictionary<string, Func<IDomainEvent, object>> columns)
        {
            foreach (var column in columns)
            {
                var key = ToParameterName(column.Key);
                var value = column.Value.Invoke(@event);
                AddValueNullSafe(command,key, value);
            }
        }
        private string ToParameterName(string parameterKey)
        {
            return $"@{parameterKey.ToLower()}";
        }
        private  void AddValueNullSafe(SqlCommand command, string key, object value)
        {
            command.Parameters.AddWithValue(key, value ?? DBNull.Value);
        }
    }
}
