// Nile.Api/Controllers/PostLikeController.cs
using Microsoft.AspNetCore.Mvc;
using Nile.Entities;
using Nile.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nile.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostLikeController : ControllerBase
    {
        private readonly PostLikeService _service;

        public PostLikeController(PostLikeService service)
        {
            _service = service;
        }

        // POST /api/postlike/{postId}/like
        [HttpPost("{postId:guid}/like")]
        public async Task<IActionResult> Like(Guid postId, [FromBody] UserActionRequest body)
        {
            try
            {
                await _service.LikeAsync(postId, body.UserId);
                return NoContent();
            }
            catch (InvalidOperationException ex) { return NotFound(new { error = ex.Message }); }
            catch (ArgumentException ex)        { return BadRequest(new { error = ex.Message }); }
        }

        // POST /api/postlike/{postId}/unlike
        [HttpPost("{postId:guid}/unlike")]
        public async Task<IActionResult> Unlike(Guid postId, [FromBody] UserActionRequest body)
        {
            await _service.UnlikeAsync(postId, body.UserId);
            return NoContent();
        }

        // GET /api/postlike/{postId}/count
        [HttpGet("{postId:guid}/count")]
        public async Task<ActionResult<object>> Count(Guid postId)
        {
            var count = await _service.CountAsync(postId);
            return Ok(new { postId, likeCount = count });
        }

        // GET /api/postlike/{postId}/recent?take=10
        [HttpGet("{postId:guid}/recent")]
        public async Task<ActionResult<IReadOnlyList<UserLikeDto>>> Recent(Guid postId, [FromQuery] int take = 10)
        {
            var users = await _service.RecentLikersAsync(postId, take);
            var dto = users.Select(u => new UserLikeDto
            {
                UserId = u.UserId,
                DisplayName = u.DisplayName,
                AvatarUrl = u.AvatarUrl
            });
            return Ok(dto);
        }
    }

}
