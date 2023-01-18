using Kwetter.UserService.Messaging;
using Kwetter.UserService.Model;
using Kwetter.UserService.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Kwetter.UserService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowKweet : ControllerBase
    {

        private readonly IFollowRepository _followRepository;
        private readonly IMessageProducer _messageProducer;

        public FollowKweet(IFollowRepository followRepository, IMessageProducer messageProducer)
        {
            _followRepository = followRepository ?? throw new ArgumentNullException(nameof(followRepository));
            _messageProducer = messageProducer ?? throw new ArgumentNullException(nameof(_messageProducer));
        }



        [HttpPost("follow")]
        public async Task<IActionResult> Follow([FromBody] Guid user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var follow = new Follow();
            follow.UserId = Guid.Parse(HttpContext.Request.Headers["claims_id"]);
            follow.FollowingUserId = user;

            if (!await _followRepository.IsFollowing(follow.UserId, follow.FollowingUserId))
            {
                await _followRepository.FollowUser(follow);
                _messageProducer.SendingMessage<Follow>(follow);
                return Ok(follow);

            }

            return BadRequest("Already following user");

        }


       
    }
}
