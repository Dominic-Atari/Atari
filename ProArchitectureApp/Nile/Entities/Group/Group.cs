/*Group
├─ GroupId (PK)
├─ Name
├─ Description
├─ OwnerUserId (FK -> User)
├─ CreatedAt
├─ Members       [many-to-many via GroupMember]
└─ Posts         [1-to-many -> Post (group posts)]

GroupMember
├─ GroupMemberId (PK)
├─ GroupId (FK -> Group)
├─ UserId  (FK -> User)
├─ Role ("member","admin","mod")
├─ JoinedAt

RELATIONS:
Group (many) ── (many) User THROUGH GroupMember
Group (1) ── (many) Post
*/
namespace Nile.Entities
{
    using System;
    using System.Collections.Generic;

     public class Group
    {
        public Guid GroupId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public Guid OwnerUserId { get; set; }
        public DateTime CreatedAt { get; set; }

        // nav
        public User OwnerUser { get; set; } = default!;

        public ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();

        // We'll add Posts under a Group later when we add GroupId to Post.
        // public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}