// Nile.Service/Dtos/CommentDtos.cs
namespace Nile.Service.Dtos
{
    public sealed class CreateCommentRequest
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public string ContentText { get; set; } = string.Empty;
        public Guid? ParentCommentId { get; set; }
    }
}
