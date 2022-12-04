namespace Kwetter.KweetService.Messaging
{
    public interface IMessageProducer
    {
        public void SendingMessage<T>(T message);
    }
}
