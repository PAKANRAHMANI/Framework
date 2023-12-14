namespace Framework.EventProcessor.Initial;

internal interface IEnableSecondSenderBuilder
{
    ISecondaryDeliveryEvent EnableSendingMessageToSecondaryBroker();
    IEventConsumer DisableSendingMessageToSecondaryBroker();
}