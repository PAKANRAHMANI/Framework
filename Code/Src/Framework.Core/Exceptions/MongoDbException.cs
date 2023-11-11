namespace Framework.Core.Exceptions
{
    public class MongoDbException : InfrastructureException
    {
        public MongoDbException():base(FrameworkErrorCodes.MongoDbNotAcknowledged,FrameworkException.MongoDBOperationNotAcknowledged)
        {
            
        }
    }
}
