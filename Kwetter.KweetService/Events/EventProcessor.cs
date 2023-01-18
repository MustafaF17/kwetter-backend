using Kwetter.KweetService.Dto;
using Kwetter.KweetService.Model;
using Kwetter.KweetService.Repository.Interface;
using System.Text.Json;

namespace Kwetter.KweetService.Events
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
            CreateFollowToKweetService(message);
        }



        private void CreateFollowToKweetService(string message)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IFollowRepository>();

                var followDto = JsonSerializer.Deserialize<FollowDto>(message);

                Follow follow = new Follow();
                follow.FollowingUserId = followDto.FollowingUserId;
                follow.UserId = followDto.UserId;
                Console.WriteLine(message);

                try
                {
                    repo.FollowUser(follow);
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
        FollowCreated,
        Undefined
    }
}
