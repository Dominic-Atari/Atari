using Microsoft.AspNetCore.Mvc;
using Nile.ResponseDTOs.FriendRequestDto;
using Nile.Service;
using Nile.Service.Dtos;

namespace Nile.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendController : ControllerBase
    {
        private readonly FriendService _service;
        public FriendController(FriendService service) => _service = service;

        // POST /api/friend/request
        [HttpPost("request")]
        public new async Task<ActionResult<FriendRequestDto>> Request([FromBody] SendFriendRequestDto req)
        {
            try
            {
                var dto = await _service.SendAsync(req);
                return Ok(dto);
            }
            catch (ArgumentException ex)        { return BadRequest(new { error = ex.Message }); }
            catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
        }

        // POST /api/friend/{relationshipId}/accept/{actingUserId}
        [HttpPost("{relationshipId:guid}/accept/{actingUserId:guid}")]
        public async Task<IActionResult> Accept(Guid relationshipId, Guid actingUserId)
        {
            try
            {
                await _service.AcceptAsync(relationshipId, actingUserId);
                return NoContent();
            }
            catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
        }

        // GET /api/friend/pending/{userId}
        [HttpGet("pending/{userId:guid}")]
        public async Task<ActionResult<IEnumerable<FriendRequestDto>>> Pending(Guid userId)
        {
            var dto = await _service.GetPendingAsync(userId);
            return Ok(dto);
        }

        // GET /api/friend/list/{userId}
        [HttpGet("list/{userId:guid}")]
        public async Task<ActionResult<IEnumerable<FriendDto>>> List(Guid userId)
        {
            var dto = await _service.GetFriendsAsync(userId);
            return Ok(dto);
        }
    }
}
