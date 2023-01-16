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
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.UserCreated:
                    CreateUserToKweetService(message);
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

            var eventType = JsonSerializer.Deserialize<UserDto>(notificationMessage);

            switch (eventType.EventType)
            {
                case "UserCreated":
                    Console.WriteLine("User Created Event Detected");
                    return EventType.UserCreated;
                default:
                    Console.WriteLine("Could not determine event type");
                    return EventType.Undefined;
            }
        }

        private void CreateUserToKweetService(string userMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IUserRepository>();

                var userDto = JsonSerializer.Deserialize<UserDto>(userMessage);
                Console.WriteLine(userMessage);

                try
                {
                    repo.CreateUser(userDto);
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
        UserCreated,
        Undefined
    }
}
