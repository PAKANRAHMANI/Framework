using System;

namespace Framework.Core.Exceptions
{
    public class EntityNotFoundException<TKey> : BusinessException
    {
        public EntityNotFoundException(string entityName, TKey id) : base($"Entity '{entityName}' not found with Id : {id}")
        {
        }
        public EntityNotFoundException(long boundedContextCode, TKey id) : base(-1, $"Entity not found with Id : {id}", boundedContextCode)
        {

        }
        public EntityNotFoundException(long boundedContextCode, string entityName, TKey id) : base(-1, $"Entity '{entityName}' not found with Id : {id}", boundedContextCode)
        {
        }
        public EntityNotFoundException(TKey id) : base($"Entity not found with Id : {id}")
        {

        }

        public EntityNotFoundException(string entityName, TKey id, long errorCode) : base(errorCode, $"Entity '{entityName}' not found with Id : {id}")
        {
        }
        public EntityNotFoundException(string entityName, TKey id, Enum errorCode) : base(errorCode, $"Entity '{entityName}' not found with Id : {id}")
        {
        }
        public EntityNotFoundException(long boundedContextCode, string entityName, TKey id, long errorCode) : base(errorCode, $"Entity '{entityName}' not found with Id : {id}", boundedContextCode)
        {
        }
        public EntityNotFoundException(long boundedContextCode, string entityName, TKey id, Enum errorCode) : base(errorCode, $"Entity '{entityName}' not found with Id : {id}", boundedContextCode)
        {
        }
    }
}
