namespace Nile.Service.Dtos
{
     public sealed class ChangeRoleRequest
    {
        public Guid GroupId { get; set; }
        public Guid UserId { get; set; }
        public string Role { get; set; } = "member";
    }
}