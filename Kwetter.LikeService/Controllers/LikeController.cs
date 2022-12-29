using Kwetter.LikeService.Model;
using Kwetter.LikeService.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kwetter.LikeService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly ILikeRepository _likeRepository;

        public LikeController(ILikeRepository likeRepository)
        {
            _likeRepository = likeRepository ?? throw new ArgumentNullException(nameof(likeRepository));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetLikeById(int id)
        {
            Like like = await _likeRepository.GetLikeById(id);
            return Ok(like);
        }

        [HttpGet("GetUserLikes")]
        public async Task<IActionResult> GetUserLikes()
        {
                var UserId = Guid.Parse(HttpContext.Request.Headers["claims_id"]);
                List<Like> likes = await _likeRepository.GetLikesByUser(UserId);
                return Ok(likes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLike([FromBody] int kweetId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var like = new Like();
            like.UserId = Guid.Parse(HttpContext.Request.Headers["claims_id"]);
            like.KweetId = kweetId;

            await _likeRepository.CreateLike(like);
            return Ok(like);
        }

        [HttpDelete("{likeId}")]
        public async Task<IActionResult> DeleteLike(int likeId)
        {
            //Check if tweet exists & tweet belongs to logged in user or admin
            var likeDetails = await _likeRepository.GetLikeById(likeId);

            Guid loggedUserId = Guid.Parse(HttpContext.Request.Headers["claims_id"]);

            if (likeDetails != null && likeDetails.UserId == loggedUserId)
            {

                var unlike = await _likeRepository.DeleteLike(likeId);
                return Ok(unlike);
            }

            else if (likeDetails == null)
            {
                return BadRequest("Like details does not exist");
            }

            else if (likeDetails.UserId != loggedUserId)
            {
                return BadRequest("Unauthorized to unlike tweet");
            }

            return BadRequest();
        }

        [HttpDelete("DeleteByKweet/{kweetId}")]
        public async Task<IActionResult> DeleteAllLikesByTweetId(int kweetId)
        {
            var deleted = await _likeRepository.DeleteByKweetId(kweetId);
            return Ok(deleted);
        }


    }
}
