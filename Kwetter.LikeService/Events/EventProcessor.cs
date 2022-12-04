using Kwetter.LikeService.Dto;
using Kwetter.LikeService.Model;
using Kwetter.LikeService.Repository.Interface;
using System.Text.Json;

namespace Kwetter.LikeService.Events
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public EventProcessor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.KweetDeleted:
                    DeleteLikesRelatedToKweet(message);
                    break;
                case EventType.Undefined:
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("Determining Event --->");
            
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            switch (eventType.Event)
            {
                case "KweetDeleted":
                    Console.WriteLine("Kweet Deleted Event Detected");
                    return EventType.KweetDeleted;
                default:
                    Console.WriteLine("Could not determine event type");
                    return EventType.Undefined;
            }
        }

        private void DeleteLikesRelatedToKweet(string KweetDeletedMessage)
        {
            using(var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ILikeRepository>();
                
                var kweetDeletedDto = JsonSerializer.Deserialize<KweetDeletedDto>(KweetDeletedMessage);
                Console.WriteLine(KweetDeletedMessage);

                try
                {
                    var likeid = kweetDeletedDto.Id;
                    repo.DeleteByKweetId(likeid);
                    Console.WriteLine("Deleted all likes matching kweet ID");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not delete from database : " + ex.Message);
                }
            }
        }

    }

    enum EventType
    {
        KweetDeleted,
        Undefined
    }
}
