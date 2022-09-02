using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Event;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using Framework.Core;

namespace Framework.DataAccess.NH
{
    public class SessionFactoryBuilder
    {
        private Assembly _mappingAssembly;
        public string ConnectionString { get;private set; }
        private string _sessionFactoryName;
        private IDomainEventPersistenceBuilder _commandBuilder;
        public SessionFactoryBuilder WithMappingsInAssembly(Assembly assembly)
        {
            _mappingAssembly = assembly;
            return this;
        }

        public SessionFactoryBuilder UsingConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
            return this;
        }

        public SessionFactoryBuilder SetSessionNameAs(string name)
        {
            _sessionFactoryName = name;
            return this;
        }
        public SessionFactoryBuilder PersistDomainEvents(IDomainEventPersistenceBuilder commandBuilder)
        {
            _commandBuilder = commandBuilder;
            return this;
        }
        private void AddDomainEventListener(Configuration configuration)
        {
            if (_commandBuilder == null) return;
            var listener = new NhDomainEventPersistListener(_commandBuilder);
            configuration.SetListener(ListenerType.PreInsert, listener);
            configuration.SetListener(ListenerType.PreUpdate, listener);
            configuration.SetListener(ListenerType.PreDelete, listener);

        }
        public ISessionFactory Build()
        {
            var configuration = new Configuration();
            configuration.SessionFactoryName(_sessionFactoryName);
            configuration.DataBaseIntegration(db =>
            {
                db.Dialect<MsSql2012Dialect>();
                db.Driver<YekeSqlClientDriver>();
                db.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                db.IsolationLevel = IsolationLevel.ReadCommitted;
                db.ConnectionString = ConnectionString;
                db.Timeout = 30;
            });

            configuration.AddAssembly(_mappingAssembly);
            var modelMapper = new ModelMapper();
            modelMapper.BeforeMapClass += (mi, t, map) => map.DynamicUpdate(true);
            modelMapper.AddMappings(_mappingAssembly.GetExportedTypes());

            AddDomainEventListener(configuration);

            var mappingDocument = modelMapper.CompileMappingForAllExplicitlyAddedEntities();
            configuration.AddDeserializedMapping(mappingDocument, _sessionFactoryName);
            SchemaMetadataUpdater.QuoteTableAndColumns(configuration, new MsSql2012Dialect());
            return configuration.BuildSessionFactory();
        }
    }
}
