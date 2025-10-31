using Microsoft.AspNetCore.Mvc;
using Nile.Entities;
using Nile.ResponseDTOs;
using Nile.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nile.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;

        public PostController(PostService postService)
        {
            _postService = postService;
        }

        // GET api/post/recent?take=5
        [HttpGet("recent")]
        public async Task<ActionResult<IReadOnlyList<Post>>> GetRecent([FromQuery] int take = 5)
        {
            var posts = await _postService.GetRecentPostsAsync(take);
            return Ok(posts);
        }

        // GET api/post/feed/{userId}?skip=0&take=20
        [HttpGet("feed/{userId:guid}")]
        public async Task<ActionResult<IReadOnlyList<Post>>> GetFeed(
            Guid userId,
            [FromQuery] int skip = 0,
            [FromQuery] int take = 20)
        {
            var posts = await _postService.GetFeedForUserAsync(userId, skip, take);
            return Ok(posts);
        }

        // GET api/post/{postId}
        [HttpGet("{postId:guid}")]
        public async Task<ActionResult<Post?>> GetDetails(Guid postId)
        {
            var post = await _postService.GetPostWithDetailsAsync(postId);
            if (post == null)
                return NotFound();
            return Ok(post);
        }

        // POST api/post
        // body: { userId, contentText, mediaUrl }
        [HttpPost]
public async Task<ActionResult<PostResponseDTO>> Create([FromBody] CreatePostRequest req)
{
    try
    {
        var post = await _postService.CreatePostAsync(
            req.UserId,
            req.ContentText,
            req.MediaUrl
        );

        var response = new PostResponseDTO
        {
            PostId = post.PostId,
            UserId = post.UserId,
            ContentText = post.ContentText,
            MediaUrl = post.MediaUrl,
            CreatedAt = post.CreatedAt,
            UserDisplayName = post.User?.DisplayName,    // might be null if not included
            UserAvatarUrl = post.User?.AvatarUrl
        };

        return Ok(response);
    }
    catch (ArgumentException ex)
    {
        return BadRequest(new { error = ex.Message });
    }
    catch (InvalidOperationException ex)
    {
        return NotFound(new { error = ex.Message });
    }
}
        // POST api/post/{postId}/like
        // body: { userId }
        [HttpPost("{postId:guid}/like")]
        public async Task<IActionResult> Like(Guid postId, [FromBody] UserActionRequest body)
        {
            try
            {
                await _postService.LikePostAsync(postId, body.UserId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        // POST api/post/{postId}/unlike
        // body: { userId }
        [HttpPost("{postId:guid}/unlike")]
        public async Task<IActionResult> Unlike(Guid postId, [FromBody] UserActionRequest body)
        {
            await _postService.UnlikePostAsync(postId, body.UserId);
            return NoContent();
        }

        // POST api/post/{postId}/comment
        // body: { userId, text }
        [HttpPost("{postId:guid}/comment")]
        public async Task<ActionResult<Comment>> Comment(Guid postId, [FromBody] AddCommentRequest body)
        {
            try
            {
                var comment = await _postService.AddCommentAsync(postId, body.UserId, body.Text);
                return Ok(comment);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }

    public class CreatePostRequest
    {
        public Guid UserId { get; set; }
        public string ContentText { get; set; } = string.Empty;
        public string? MediaUrl { get; set; }
    }

    public class UserActionRequest
    {
        public Guid UserId { get; set; }
    }

    public class AddCommentRequest
    {
        public Guid UserId { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
