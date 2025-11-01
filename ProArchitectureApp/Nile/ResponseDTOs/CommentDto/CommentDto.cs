namespace Nile.Service.Dtos
{
    public sealed class CommentDto
    {
        public Guid CommentId { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public string AuthorDisplayName { get; set; } = string.Empty;
        public string? AuthorAvatarUrl { get; set; }
        public string ContentText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public Guid? ParentCommentId { get; set; }
    }
}