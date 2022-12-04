using System.ComponentModel.DataAnnotations;

namespace Kwetter.KweetService.Model
{
    public class Follow
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid FollowingUserId { get; set; }

    }
}
