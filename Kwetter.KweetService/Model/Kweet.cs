using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kwetter.KweetService.Model
{
    public class Kweet
    {

        [Key]
        [Required]
        public int Id { get; set; }
        
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public string Text { get; set; }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

    }
}
