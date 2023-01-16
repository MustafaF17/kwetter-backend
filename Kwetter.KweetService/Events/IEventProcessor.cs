namespace Kwetter.KweetService.Events
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}
