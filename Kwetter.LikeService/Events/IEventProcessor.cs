namespace Kwetter.LikeService.Events
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}
