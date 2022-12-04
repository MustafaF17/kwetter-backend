using Kwetter.UserService.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Kwetter.UserService.Model;

namespace Kwetter.UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            List<User> users = await _userRepository.GetAllUsers();
            return Ok(users);
        }


        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            User user = await _userRepository.GetUserById(userId);
            return Ok(user);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            bool deleted = await _userRepository.DeleteUser(id);
            return Ok(deleted);
        }


    }
}
