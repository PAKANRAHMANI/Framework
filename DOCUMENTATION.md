# Framework Documentation

## Table of Contents
1. [Overview](#overview)
2. [Architecture](#architecture)
3. [Core Components](#core-components)
4. [Data Access](#data-access)
5. [Messaging](#messaging)
6. [Caching](#caching)
7. [Security](#security)
8. [Monitoring & Logging](#monitoring--logging)
9. [File Management](#file-management)
10. [API Documentation](#api-documentation)

### Overview
The Framework is a comprehensive .NET-based solution that provides a robust foundation for building enterprise-level applications. It implements various design patterns and best practices to ensure scalability, maintainability, and security. This framework is designed to handle complex business requirements while maintaining clean architecture principles and providing a rich set of features for modern application development.

### Architecture
The framework follows a modular architecture with the following main components:

#### Core Components

##### Framework.Core
The core module provides fundamental functionality and interfaces that are used throughout the framework.

**Key Features:**
- Unit of Work Pattern Implementation
  ```csharp
  public interface IUnitOfWork : IDisposable
  {
      Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
      Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
  }
  ```
  - Manages database transactions
  - Ensures data consistency
  - Handles entity tracking
  - Provides atomic operations

- Domain Event Handling
  ```csharp
  public interface IDomainEventDispatcher
  {
      Task DispatchEventsAsync(IEnumerable<IDomainEvent> events);
  }
  ```
  - Event publishing
  - Event subscription
  - Event persistence
  - Event replay capabilities

- Environment Configuration
  ```csharp
  public static class EnvironmentHelper
  {
      public static bool IsDevelopment() => 
          Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
  }
  ```
  - Environment-specific settings
  - Configuration management
  - Feature flags
  - Environment variables

- Logging Infrastructure
  ```csharp
  public interface ILogger
  {
      void LogInformation(string message);
      void LogError(Exception ex, string message);
      void LogWarning(string message);
  }
  ```
  - Structured logging
  - Log levels
  - Log enrichment
  - Log shipping

##### Framework.Domain
The domain module contains the core business logic and domain models.

**Key Features:**
- Entity Definitions
  ```csharp
  public abstract class Entity
  {
      public Guid Id { get; protected set; }
      public DateTime CreatedAt { get; protected set; }
      public DateTime? UpdatedAt { get; protected set; }
  }
  ```
  - Base entity class
  - Identity management
  - Audit fields
  - Entity lifecycle

- Value Objects
  ```csharp
  public abstract class ValueObject
  {
      protected abstract IEnumerable<object> GetEqualityComponents();
      public override bool Equals(object obj) { /* implementation */ }
      public override int GetHashCode() { /* implementation */ }
  }
  ```
  - Immutable objects
  - Value equality
  - Domain validation
  - Business rules

- Domain Events
  ```csharp
  public interface IDomainEvent
  {
      DateTime OccurredOn { get; }
      Guid EventId { get; }
  }
  ```
  - Event definition
  - Event publishing
  - Event handling
  - Event persistence

##### Framework.Application
The application module implements the use cases and business logic orchestration.

**Key Features:**
- Command Handlers
  ```csharp
  public interface ICommandHandler<TCommand> where TCommand : ICommand
  {
      Task<Result> HandleAsync(TCommand command);
  }
  ```
  - Command validation
  - Business logic execution
  - Transaction management
  - Result handling

- Query Handlers
  ```csharp
  public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
  {
      Task<TResult> HandleAsync(TQuery query);
  }
  ```
  - Query execution
  - Data retrieval
  - Result mapping
  - Caching integration

#### Data Access Layer

##### Framework.DataAccess.EF
Entity Framework implementation for relational databases.

**Key Features:**
- Repository Pattern
  ```csharp
  public interface IRepository<T> where T : Entity
  {
      Task<T> GetByIdAsync(Guid id);
      Task<IEnumerable<T>> GetAllAsync();
      Task AddAsync(T entity);
      Task UpdateAsync(T entity);
      Task DeleteAsync(T entity);
  }
  ```
  - CRUD operations
  - Query specifications
  - Entity tracking
  - Bulk operations

- Unit of Work
  ```csharp
  public class UnitOfWork : IUnitOfWork
  {
      private readonly DbContext _context;
      public async Task<int> SaveChangesAsync() => 
          await _context.SaveChangesAsync();
  }
  ```
  - Transaction management
  - Change tracking
  - Entity state management
  - Save changes

##### Framework.DataAccess.Mongo
MongoDB implementation for document storage.

**Key Features:**
- Document Repositories
  ```csharp
  public interface IMongoRepository<T> where T : Document
  {
      Task<T> FindOneAsync(FilterDefinition<T> filter);
      Task<IEnumerable<T>> FindManyAsync(FilterDefinition<T> filter);
      Task InsertOneAsync(T document);
      Task UpdateOneAsync(FilterDefinition<T> filter, UpdateDefinition<T> update);
  }
  ```
  - Document operations
  - Query building
  - Index management
  - Aggregation support

#### Messaging & Event Processing

##### Framework.MassTransit
Message bus implementation for distributed systems.

**Key Features:**
- Message Routing
  ```csharp
  public interface IMessageBus
  {
      Task PublishAsync<T>(T message) where T : class;
      Task SubscribeAsync<T>(Func<T, Task> handler) where T : class;
  }
  ```
  - Message publishing
  - Message consumption
  - Message routing
  - Error handling

##### Framework.RabbitMQ
RabbitMQ integration for message queuing.

**Key Features:**
- Queue Management
  ```csharp
  public interface IRabbitMQClient
  {
      Task PublishAsync(string exchange, string routingKey, byte[] message);
      Task ConsumeAsync(string queue, Func<byte[], Task> handler);
  }
  ```
  - Queue operations
  - Exchange management
  - Message persistence
  - Dead letter handling

#### Caching

##### Framework.Caching
Core caching functionality.

**Key Features:**
- Cache Interfaces
  ```csharp
  public interface ICache
  {
      Task<T> GetAsync<T>(string key);
      Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
      Task RemoveAsync(string key);
  }
  ```
  - Cache operations
  - Cache strategies
  - Cache invalidation
  - Distributed caching

##### Framework.Redis
Redis implementation for distributed caching.

**Key Features:**
- Redis Operations
  ```csharp
  public interface IRedisClient
  {
      Task<T> GetAsync<T>(string key);
      Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
      Task<bool> RemoveAsync(string key);
  }
  ```
  - Data structures
  - Pub/Sub
  - Cluster support
  - Connection management

#### Security & Authentication

##### Framework.Security
Security implementations.

**Key Features:**
- Encryption
  ```csharp
  public interface IEncryption
  {
      string Encrypt(string plainText, string key);
      string Decrypt(string cipherText, string key);
  }
  ```
  - Encryption/Decryption
  - Hash functions
  - Security protocols
  - Token management

##### Framework.Authentication
Authentication services.

**Key Features:**
- JWT Handling
  ```csharp
  public interface IJwtService
  {
      string GenerateToken(User user);
      bool ValidateToken(string token);
      ClaimsPrincipal GetPrincipal(string token);
  }
  ```
  - Token generation
  - Token validation
  - Claims management
  - Session handling

#### Monitoring & Logging

##### Framework.HealthCheck
Health monitoring system.

**Key Features:**
- Health Checks
  ```csharp
  public interface IHealthCheck
  {
      Task<HealthCheckResult> CheckHealthAsync();
  }
  ```
  - Service health
  - Dependency checks
  - Performance metrics
  - Status reporting

##### Framework.Logging.SeriLog
Logging implementation.

**Key Features:**
- Logging
  ```csharp
  public interface ILogger
  {
      void Information(string message);
      void Warning(string message);
      void Error(Exception ex, string message);
  }
  ```
  - Structured logging
  - Log levels
  - Log enrichment
  - Log shipping

### Getting Started

#### Prerequisites
- .NET SDK (version specified in global.json)
- Docker (for containerized services)
- Redis (for caching)
- MongoDB (for document storage)
- ClickHouse (for analytics)

#### Installation
1. Clone the repository
2. Install dependencies
3. Configure services in appsettings.json
4. Run the application

#### Basic Usage
```csharp
// Example of using the framework
services.AddFrameworkCore()
       .AddFrameworkCaching()
       .AddFrameworkSecurity();
```

### Best Practices
1. Follow SOLID principles
   - Single Responsibility Principle
   - Open/Closed Principle
   - Liskov Substitution Principle
   - Interface Segregation Principle
   - Dependency Inversion Principle

2. Use dependency injection
   - Constructor injection
   - Property injection
   - Method injection
   - Service lifetime management

3. Implement proper error handling
   - Exception hierarchies
   - Error logging
   - Error recovery
   - User feedback

4. Write unit tests
   - Test coverage
   - Mocking
   - Test isolation
   - Test data management

5. Use async/await for I/O operations
   - Task management
   - Cancellation support
   - Exception handling
   - Performance optimization

### API Documentation
Detailed API documentation for each component is available in the respective project directories. Each component includes:
- Interface definitions
- Implementation details
- Usage examples
- Configuration options
- Best practices
- Performance considerations 