using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Kwetter.KweetService.Dto
{
    public class KweetDto
    {
        public KweetDto(int id, string @event)
        {
            Id = id;
            Event = @event;
        }

        public int Id { get; set; }
        public string Event { get; set; }




    }
}
