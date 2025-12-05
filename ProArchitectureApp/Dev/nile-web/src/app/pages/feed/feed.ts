import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PostService } from '../../services/post.service';
import { Post } from '../../models/post';

@Component({
  selector: 'app-feed',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './feed.html',
  styleUrl: './feed.scss',
})
export class Feed implements OnInit {
  posts: Post[] = [];
  loading = false;
  saving = false;

  newPost = {
    userId: '',
    contentText: '',
    mediaUrl: '' as string | null,
  };

  constructor(private postsApi: PostService) {}

  ngOnInit(): void {
    this.loading = true;
    this.postsApi.getRecent(10).subscribe({
      next: (data) => (this.posts = data),
      error: () => {},
      complete: () => (this.loading = false),
    });
  }

  trackById = (_: number, p: Post) => p.postId;

  submit(): void {
    if (!this.newPost.userId || !this.newPost.contentText.trim()) {
      return;
    }
    this.saving = true;
    const req = {
      userId: this.newPost.userId,
      contentText: this.newPost.contentText.trim(),
      mediaUrl: this.newPost.mediaUrl || null,
    };
    this.postsApi.createPost(req).subscribe({
      next: (created) => {
        this.posts = [created, ...this.posts];
        this.newPost.contentText = '';
        this.newPost.mediaUrl = '';
      },
      error: () => {},
      complete: () => (this.saving = false),
    });
  }
}
