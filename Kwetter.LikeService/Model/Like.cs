using System.ComponentModel.DataAnnotations;

namespace Kwetter.LikeService.Model
{
    public class Like
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public int KweetId { get; set; }
    }
}
