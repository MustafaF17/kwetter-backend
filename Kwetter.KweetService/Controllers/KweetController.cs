using Kwetter.KweetService.Dto;
using Kwetter.KweetService.Messaging;
using Kwetter.KweetService.Model;
using Kwetter.KweetService.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Kwetter.KweetService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KweetController : ControllerBase
    {
        private readonly IKweetRepository _kweetRepository;

        public KweetController(IKweetRepository kweetRepository)
        {
            _kweetRepository = kweetRepository ?? throw new ArgumentNullException(nameof(kweetRepository));
        }

        [HttpGet("Feed")]
        public async Task<IActionResult> GetFeed()
        {
        
            List<Kweet> feed = await _kweetRepository.GetAllKweets();
            return Ok(feed);
        }

        [HttpGet("FollowingFeed")]
        public async Task<IActionResult> FollowingFeed()
        {
            var userId = Guid.Parse(HttpContext.Request.Headers["claims_id"]);
            List<Kweet> feed = await _kweetRepository.GetFollowingKweets(userId);
            return Ok(feed);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetKweetById(int id)
        {
           Kweet kweet = await _kweetRepository.GetKweetById(id);
            return Ok(kweet);
        }

        [HttpGet("KweetByUser/{userId}")]
        public async Task <IActionResult> GetKweetByUser(Guid userId)
        {
            List<Kweet> kweets = await _kweetRepository.GetKweetsByUser(userId);
            return Ok(kweets);

        }

        [HttpPost]
        public async Task <IActionResult> CreateKweet([FromBody] string text)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            var kweet = new Kweet();
            kweet.UserId = Guid.Parse(HttpContext.Request.Headers["claims_id"]);
            kweet.Username = HttpContext.Request.Headers["claims_username"];
            kweet.Text = text;
            kweet.Created = DateTime.Now;

            await _kweetRepository.CreateKweet(kweet);
            return Ok(kweet);
        }

    }
}
