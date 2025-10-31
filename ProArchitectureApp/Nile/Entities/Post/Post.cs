using System;
using System.Collections.Generic;

namespace Nile.Entities
{
    public class Post
    {
        public Guid PostId { get; set; }

        public Guid UserId { get; set; }

        public string ContentText { get; set; } = string.Empty;
        public string? MediaUrl { get; set; } // image/video/etc.
        public DateTime CreatedAt { get; set; }

        // navs
        public User User { get; set; } = default!;

        public ICollection<PostLike> Likes { get; set; } = new List<PostLike>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        // We are NOT including GroupId/Group yet to keep first migration simpler.
        // We'll extend Post later if you want posts inside groups.
    }
}
