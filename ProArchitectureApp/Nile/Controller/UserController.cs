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
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        // POST /api/user
        // body: { displayName, email, passwordHash, avatarUrl }
        [HttpPost]
        public async Task<ActionResult<User>> Create([FromBody] CreateUserRequest body)
        {
            try
            {
                var created = await _userService.CreateUserAsync(
                    body.DisplayName,
                    body.Email,
                    body.PasswordHash,
                    body.AvatarUrl
                );

                return Ok(created);
            }
            catch (InvalidOperationException ex)
            {
                // e.g. "Email already in use."
                return Conflict(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                // e.g. missing displayName / email, etc.
                return BadRequest(new { error = ex.Message });
            }
        }

        // GET /api/user/{userId}
        [HttpGet("{userId:guid}")]
        public async Task<ActionResult<User>> GetById(Guid userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // GET /api/user/{userId}/friends
        [HttpGet("{userId:guid}/friends")]
        public async Task<ActionResult<IReadOnlyList<User>>> GetFriends(Guid userId)
        {
            var friends = await _userService.GetFriendsAsync(userId);
            return Ok(friends);
        }

        // GET /api/user/{userId}/requests
        // This returns pending friend requests sent TO this user
        [HttpGet("{userId:guid}/requests")]
        public async Task<ActionResult<IReadOnlyList<FriendRelationship>>> GetIncomingRequests(Guid userId)
        {
            var requests = await _userService.GetIncomingFriendRequestsAsync(userId);
            return Ok(requests);
        }

        // GET /api/user/areFriends?userA=...&userB=...
        [HttpGet("areFriends")]
        public async Task<ActionResult<object>> AreFriends(
            [FromQuery] Guid userA,
            [FromQuery] Guid userB)
        {
            var result = await _userService.AreFriendsAsync(userA, userB);
            return Ok(new { areFriends = result });
        }
    }

    // DTO for creating a user
    public class CreateUserRequest
    {
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Right now you're passing PasswordHash directly
        // (Later we can accept plain password and hash in service)
        public string PasswordHash { get; set; } = string.Empty;

        public string? AvatarUrl { get; set; }
    }
}
