// Nile.Service/Dtos/GroupDtos.cs
namespace Nile.Service.Dtos
{
    public sealed class CreateGroupRequest
    {
        public Guid OwnerUserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
