using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Kwetter.KweetService.Dto
{
    public class FollowDto
    {

        public int Id { get; set; }
        public Guid UserId { get; set; }
        public Guid FollowingUserId { get; set; }

        public FollowDto(int id, Guid userId, Guid followingUserId)
        {
            Id = id;
            UserId = userId;
            FollowingUserId = followingUserId;
        }
    }
}
