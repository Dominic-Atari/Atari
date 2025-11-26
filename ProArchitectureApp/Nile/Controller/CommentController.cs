// Nile.Api/Controllers/CommentController.cs
using Microsoft.AspNetCore.Mvc;
using Nile.Service;
using Nile.Service.Dtos;

namespace Nile.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly CommentService _service;
        public CommentController(CommentService service) => _service = service;

        // POST /api/comment
        [HttpPost]
        public async Task<ActionResult<CommentDto>> Create([FromBody] CreateCommentRequestDto req)
        {
            try
            {
                var dto = await _service.CreateAsync(req);
                return Ok(dto);
            }
            catch (ArgumentException ex)        { return BadRequest(new { error = ex.Message }); }
            catch (InvalidOperationException ex) { return BadRequest(new { error = ex.Message }); }
        }

        // GET /api/comment/posts/{postId}/comments?skip=0&take=50
        [HttpGet("posts/{postId:guid}/comments")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsOfAPost(
            Guid postId, [FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            var dto = await _service.GetForPostAsync(postId, skip, take);
            return Ok(dto);
        }


        // GET /api/comment/{commentId}/replies
        [HttpGet("{commentId:guid}/replies")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> Replies(Guid commentId)
        {
            var dto = await _service.GetRepliesAsync(commentId);
            return Ok(dto);
        }

        // DELETE /api/comment/{commentId}/{requestingUserId}
        [HttpDelete("{commentId:guid}/{requestingUserId:guid}")]
        public async Task<IActionResult> Delete(Guid commentId, Guid requestingUserId)
        {
            try
            {
                await _service.DeleteAsync(commentId, requestingUserId);
                return NoContent();
            }
            catch (InvalidOperationException ex) { return StatusCode(403, new { error = ex.Message }); }
        }
    }
}
