using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Kwetter.UserService.Dto
{
    public class UserDto
    {

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string EventType { get; set; }

        public UserDto(Guid id, string username, string role, string eventType)
        {
            Id = id;
            Username = username;
            Role = role;
            EventType = eventType;
        }
    }
}
