/*PostLike
├─ PostLikeId (PK)
├─ PostId (FK -> Post)
├─ UserId (FK -> User)
├─ CreatedAt

RELATIONS:
User (many) ── (many) Post THROUGH PostLike
- A user can like many posts
- A post can be liked by many users
*/
namespace Nile.Entities
{
    public class PostLike
    {
        public Guid PostLikeId { get; set; }

        public Guid PostId { get; set; }
        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        // navs
        public Post Post { get; set; } = default!;
        public User User { get; set; } = default!;
    }
}
