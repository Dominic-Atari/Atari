// Nile.Entities/Comment.cs
using System;
using System.Collections.Generic;

namespace Nile.Entities
{
    public class Comment
    {
        public Guid CommentId { get; set; }

        public Guid PostId { get; set; }
        public Guid UserId { get; set; }

        public string ContentText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // NEW: threaded replies
        public Guid? ParentCommentId { get; set; }
        public Comment? ParentComment { get; set; }
        public ICollection<Comment> Replies { get; set; } = new List<Comment>();

        // navs
        public Post Post { get; set; } = default!;
        public User User { get; set; } = default!;
    }
}
