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
        private readonly IMessageProducer _messageProducer;

        public KweetController(IKweetRepository kweetRepository, IMessageProducer messageProducer)
        {
            _kweetRepository = kweetRepository ?? throw new ArgumentNullException(nameof(kweetRepository));
            _messageProducer = messageProducer ?? throw new ArgumentNullException(nameof(_messageProducer));
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
            kweet.Text = text;

            await _kweetRepository.CreateKweet(kweet);
            return Ok(kweet);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKweet(int id)
        {
            //Check if tweet exists & tweet belongs to logged in user or admin
            var kweet = await _kweetRepository.GetKweetById(id);

            Guid loggedUserId = Guid.Parse(HttpContext.Request.Headers["claims_id"]);
            var loggedUserRole = HttpContext.Request.Headers["claims_role"];



            if (kweet != null && kweet.UserId == loggedUserId || kweet != null && loggedUserRole == "Admin")
            {
                var KweetDto = new KweetDto(id,"KweetDeleted");
                //KweetDto.Id = id;
                //KweetDto.Event = "KweetDeleted";

                var deleted = await _kweetRepository.DeleteKweet(id);
                if (deleted)
                    _messageProducer.SendingMessage<KweetDto>(KweetDto);
                return Ok(deleted);
            }

            else if (kweet == null)
            {
                return BadRequest("Kweet does not exist");
            }

            else if (kweet.UserId != loggedUserId)
            {
                return BadRequest("Unauthorized to delete tweet");
            }

            return BadRequest();
        }


    }
}
