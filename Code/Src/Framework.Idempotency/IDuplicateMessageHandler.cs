namespace Framework.Idempotency
{
    public interface IDuplicateMessageHandler
    {
        Task<bool> HasMessageBeenProcessedBefore(Guid eventId);
        Task MarkMessageAsProcessed(Guid eventId, DateTime receivedDate);
    }
}