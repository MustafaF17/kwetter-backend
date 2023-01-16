namespace Kwetter.UserService.Messaging
{
    public interface IMessageProducer
    {
        public void SendingMessage<T>(T message);
    }
}
