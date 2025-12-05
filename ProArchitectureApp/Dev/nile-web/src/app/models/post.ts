export interface Post {
  postId: string;
  userId: string;
  contentText: string;
  mediaUrl?: string | null;
  createdAt: string;
  // Optional fields if API includes nested user
  userDisplayName?: string | null;
  userAvatarUrl?: string | null;
}
