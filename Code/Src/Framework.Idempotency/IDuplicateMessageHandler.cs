namespace Framework.Idempotency
{
    public interface IDuplicateMessageHandler
    {
        Task<bool> HasMessageBeenProcessedBefore(string eventId);
        Task MarkMessageAsProcessed(string eventId);
    }
}