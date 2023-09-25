namespace Framework.EventProcessor.Initial;

public interface IEnableSecondSenderBuilder
{
    ISecondaryDeliveryEvent EnableSendingMessageToSecondaryBroker();
    IEventProcessor DisableSendingMessageToSecondaryBroker();
}