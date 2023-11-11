namespace Framework.Core.Exceptions;

public class MongoDbConcurrencyReplaceOneFailException : InfrastructureException
{
    public MongoDbConcurrencyReplaceOneFailException() : base(FrameworkErrorCodes.MongoDbConcurrencyReplaceException, FrameworkException.MongoDbConcurrencyReplaceOneFailException)
    {

    }
}