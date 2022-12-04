using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Kwetter.UserService.Dto
{
    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
