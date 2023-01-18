using Kwetter.KweetService.Dto;
using Kwetter.KweetService.Messaging;
using Kwetter.KweetService.Model;
using Kwetter.KweetService.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Kwetter.KweetService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KweetLikeController : ControllerBase
    {
        private readonly IKweetRepository _kweetRepository;
        private readonly IMessageProducer _messageProducer;

        public KweetLikeController(IKweetRepository kweetRepository, IMessageProducer messageProducer)
        {
            _kweetRepository = kweetRepository ?? throw new ArgumentNullException(nameof(kweetRepository));
            _messageProducer = messageProducer ?? throw new ArgumentNullException(nameof(_messageProducer));
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
