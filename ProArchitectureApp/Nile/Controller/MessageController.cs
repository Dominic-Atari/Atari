using Microsoft.AspNetCore.Mvc;
using Nile.Service;
using Nile.Service.Dtos;

namespace Nile.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly MessageService _service;

        public MessageController(MessageService service)
        {
            _service = service;
        }

        // POST /api/message
        [HttpPost]
        public async Task<ActionResult<MessageDto>> Send([FromBody] SendMessageRequestDto req)
        {
            try
            {
                var dto = await _service.SendAsync(req);
                return Ok(dto);
            }
            catch (ArgumentException ex)         { return BadRequest(new { error = ex.Message }); }
            catch (InvalidOperationException ex)  { return NotFound(new { error = ex.Message }); }
        }

        // GET /api/message/thread/{a}/{b}?skip=0&take=50
        [HttpGet("thread/{a:guid}/{b:guid}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> Thread(Guid a, Guid b, [FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            var dto = await _service.GetThreadAsync(a, b, skip, take);
            return Ok(dto);
        }

        // POST /api/message/{id}/read
        [HttpPost("{id:guid}/read")]
        public async Task<IActionResult> MarkRead(Guid id)
        {
            await _service.MarkReadAsync(id);
            return NoContent();
        }

        // POST /api/message/thread/{me}/{other}/read
        [HttpPost("thread/{me:guid}/{other:guid}/read")]
        public async Task<IActionResult> MarkThreadRead(Guid me, Guid other)
        {
            await _service.MarkThreadReadAsync(me, other);
            return NoContent();
        }

        // GET /api/message/inbox/{userId}?skip=0&take=50
        [HttpGet("inbox/{userId:guid}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> Inbox(Guid userId, [FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            var dto = await _service.GetInboxAsync(userId, skip, take);
            return Ok(dto);
        }
    }
}
