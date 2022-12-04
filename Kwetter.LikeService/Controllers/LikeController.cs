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

        [HttpGet("LikeByUser/{userId}")]
        public async Task<IActionResult> GetLikesByUser(Guid userId)
        {
            List<Like> likes = await _likeRepository.GetLikesByUser(userId);
            return Ok(likes);

        }

        [HttpPost]
        public async Task<IActionResult> CreateLike(Like like)
        {
            var created = await _likeRepository.CreateLike(like);
            return Ok(created);
        }

        [HttpDelete("{likeId}")]
        public async Task<IActionResult> DeleteLike(int likeId)
        {
            var deleted = await _likeRepository.DeleteLike(likeId);
            return Ok(deleted);
        }

        [HttpDelete("DeleteByKweet/{kweetId}")]
        public async Task<IActionResult> DeleteAllLikesByTweetId(int kweetId)
        {
            var deleted = await _likeRepository.DeleteByKweetId(kweetId);
            return Ok(deleted);
        }


    }
}
