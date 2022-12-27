using Kwetter.UserService.Dto;
using Kwetter.UserService.Model;
using Kwetter.UserService.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Kwetter.UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (await _userRepository.GetByUsername(registerDto.Username.Trim()) != null)
            {
                return BadRequest("User already exists");
            }

            CreatePasswordHash(registerDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new User();

            user.Id = Guid.NewGuid();
            user.Username = registerDto.Username.Trim();
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Role = "Guest";

            await _userRepository.CreateUser(user);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Username))
            {
                return BadRequest("LOGIN.MISSING_USERNAME");
            }

            else if (string.IsNullOrWhiteSpace(loginDto.Password))
            {
                return BadRequest("LOGIN.MISSING_PASSWORD");
            }

            // Get user and see if it exists
            var user = await _userRepository.GetByUsername(loginDto.Username.Trim());

            if (user == null)
            {
                return BadRequest("LOGIN.USERNAME_NOT_EXISTS");
            }

            // Verify if user credentials are correct & create token
            else if (VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt) && loginDto.Username == user.Username)
            {
                string token = CreateToken(user);
                return Ok(token);
            }

            else 
            {
                return BadRequest("Unable to verify user");
            }

        }



        // Password hashing logic

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }


        // Token builder

        private string CreateToken(User user)
        {

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("Role", user.Role),
                new Claim("Id", user.Id.ToString()),
                new Claim("Username", user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

    }
}
