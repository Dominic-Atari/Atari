using Microsoft.AspNetCore.Mvc;
using Nile.ResponseDTOs.Notifications;
using Nile.Service;

namespace Nile.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationService _service;

        public NotificationController(NotificationService service)
        {
            _service = service;
        }

        // GET /api/notification/user/{userId}?take=20
        [HttpGet("user/{userId:guid}")]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetRecent(Guid userId, [FromQuery] int take = 20)
        {
            try
            {
                var items = await _service.GetRecentAsync(userId, take);
                return Ok(items);
            }
            catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
        }

        // POST /api/notification
        [HttpPost]
        public async Task<ActionResult<NotificationDto>> Create([FromBody] CreateNotificationRequest req)
        {
            try
            {
                var dto = await _service.CreateAsync(req);
                return Ok(dto);
            }
            catch (ArgumentException ex)        { return BadRequest(new { error = ex.Message }); }
            catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
        }

        // POST /api/notification/{id}/read
        [HttpPost("{id:guid}/read")]
        public async Task<IActionResult> MarkRead(Guid id)
        {
            await _service.MarkReadAsync(id);
            return NoContent();
        }

        // POST /api/notification/user/{userId}/read-all
        [HttpPost("user/{userId:guid}/read-all")]
        public async Task<IActionResult> MarkAllRead(Guid userId)
        {
            try
            {
                await _service.MarkAllReadAsync(userId);
                return NoContent();
            }
            catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
        }

        // DELETE /api/notification/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
