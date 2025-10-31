/*Comment
├─ CommentId (PK)
├─ PostId    (FK -> Post)
├─ UserId    (FK -> User)
├─ ContentText
├─ CreatedAt
└─ ParentCommentId (nullable, FK -> Comment)  // for replies/threads

RELATIONS:
Post (1) ── (many) Comment
User (1) ── (many) Comment
Comment (1) ── (many) Comment (replies)
*/
namespace Nile.Entities
{
    public class Comment
    {
        public Guid CommentId { get; set; }

        public Guid PostId { get; set; }
        public Guid UserId { get; set; }

        public string ContentText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // navs
        public Post Post { get; set; } = default!;
        public User User { get; set; } = default!;
        // We'll add Posts under a Group later when we add GroupId to Post.
        // public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}

