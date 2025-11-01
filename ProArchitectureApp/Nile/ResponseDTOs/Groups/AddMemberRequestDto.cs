namespace Nile.Service.Dtos
{
     public sealed class AddMemberRequest
    {
        public Guid GroupId { get; set; }
        public Guid UserId { get; set; }
        public string Role { get; set; } = "member"; // "member" | "admin" | "mod"
    }
}