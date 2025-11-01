// Nile.Api/Controllers/GroupController.cs
using Microsoft.AspNetCore.Mvc;
using Nile.Service;
using Nile.Service.Dtos;

namespace Nile.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly GroupService _service;

        public GroupController(GroupService service) => _service = service;

        // POST /api/group
        [HttpPost]
        public async Task<ActionResult<GroupDto>> Create([FromBody] CreateGroupRequest req)
        {
            try
            {
                var dto = await _service.CreateAsync(req);
                return Ok(dto);
            }
            catch (ArgumentException ex)         { return BadRequest(new { error = ex.Message }); }
            catch (InvalidOperationException ex)  { return NotFound(new { error = ex.Message }); }
        }

        // GET /api/group/{groupId}
        [HttpGet("{groupId:guid}")]
        public async Task<ActionResult<GroupDto>> Get(Guid groupId)
        {
            var dto = await _service.GetAsync(groupId);
            return dto is null ? NotFound() : Ok(dto);
        }

        // GET /api/group/mygroup/{userId}
        [HttpGet("mygroup/{userId:guid}")]
        public async Task<ActionResult<IEnumerable<GroupDto>>> MyGroups(Guid userId)
        {
            var dto = await _service.MyGroupsAsync(userId);
            return Ok(dto);
        }

        // GET /api/group/{groupId}/allmembers
        [HttpGet("{groupId:guid}/allmembers")]
        public async Task<ActionResult<IEnumerable<GroupMemberDto>>> Members(Guid groupId)
        {
            var dto = await _service.GetMembersAsync(groupId);
            return Ok(dto);
        }

        // POST /api/group/{groupId}/addMember/{actingUserId}
        [HttpPost("{groupId:guid}/addMember/{actingUserId:guid}")]
        public async Task<IActionResult> AddMember(Guid groupId, Guid actingUserId, [FromBody] AddMemberRequest body)
        {
            try
            {
                if (body.GroupId == Guid.Empty) body.GroupId = groupId;
                await _service.AddMemberAsync(body, actingUserId);
                return NoContent();
            }
            catch (InvalidOperationException ex) { return Forbid(ex.Message); }
            catch (ArgumentException ex)         { return BadRequest(new { error = ex.Message }); }
            catch (KeyNotFoundException ex)      { return NotFound(new { error = ex.Message }); }
        }

        // DELETE /api/group/{groupId}/removeMember/{userId}/{actingUserId}
        [HttpDelete("{groupId:guid}/removeMember/{userId:guid}/{actingUserId:guid}")]
        public async Task<IActionResult> RemoveMember(Guid groupId, Guid userId, Guid actingUserId)
        {
            try
            {
                await _service.RemoveMemberAsync(groupId, userId, actingUserId);
                return NoContent();
            }
            catch (InvalidOperationException ex) { return Forbid(ex.Message); }
        }

        // POST /api/group/{groupId}/changeRole/{actingUserId}
        [HttpPost("{groupId:guid}/changeRole/{actingUserId:guid}")]
        public async Task<IActionResult> ChangeRole(Guid groupId, Guid actingUserId, [FromBody] ChangeRoleRequest body)
        {
            try
            {
                if (body.GroupId == Guid.Empty) body.GroupId = groupId;
                await _service.ChangeRoleAsync(body, actingUserId);
                return NoContent();
            }
            catch (InvalidOperationException ex) { return Forbid(ex.Message); }
        }
    }
}
