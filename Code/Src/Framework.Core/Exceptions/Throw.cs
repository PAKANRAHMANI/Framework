namespace Framework.Core.Exceptions;

public static class Throw
{
    public static class Infrastructure
    {
        public static void MongoDbNotAcknowledged()
        {
            throw new MongoDbException();
        }
        public static void MongoDbConcurrencyReplaceOneFail()
        {
            throw new MongoDbConcurrencyReplaceOneFailException();
        }
        
    }
}