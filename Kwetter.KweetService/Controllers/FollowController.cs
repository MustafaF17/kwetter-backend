using Kwetter.KweetService.Model;
using Kwetter.KweetService.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Kwetter.KweetService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowController : ControllerBase
    {

        private readonly IFollowRepository _followRepository;

        public FollowController(IFollowRepository followRepository)
        {
            _followRepository = followRepository ?? throw new ArgumentNullException(nameof(followRepository));
        }

        [HttpGet("Following/{userId}")]
        public async Task<IActionResult> GetFollowing(Guid userId)
        {
            List<Follow> follows = await _followRepository.GetFollowingByUser(userId);
            return Ok(follows);

        }

        [HttpGet("Followers/{userId}")]
        public async Task<IActionResult> GetFollowers(Guid userId)
        {
            List<Follow> follows = await _followRepository.GetFollowersByUser(userId);
            return Ok(follows);

        }

        [HttpPost]
        public async Task<IActionResult> Follow([FromBody] Guid user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var follow = new Follow();
            follow.UserId = Guid.Parse(HttpContext.Request.Headers["claims_id"]);
            follow.FollowingUserId = user;

            await _followRepository.FollowUser(follow);
            return Ok(follow);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Unfollow(int id)
        {
            //Check if tweet exists & tweet belongs to logged in user or admin
            var followDetails = await _followRepository.GetFollowDetailsById(id);

            Guid loggedUserId = Guid.Parse(HttpContext.Request.Headers["claims_id"]);


            if (followDetails != null && followDetails.UserId == loggedUserId)
            {
        
                var unfollow = await _followRepository.UnfollowUser(id);
                return Ok(unfollow);
            }

            else if (followDetails == null)
            {
                return BadRequest("Follow details does not exist");
            }

            else if (followDetails.UserId != loggedUserId)
            {
                return BadRequest("Unauthorized to delete tweet");
            }

            return BadRequest();
        }
    }
}
