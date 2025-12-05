import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Post } from '../models/post';
import { environment } from '../../environments/environment.development';

@Injectable({ providedIn: 'root' })
export class PostService {
  private readonly base = environment.apiBase;

  constructor(private http: HttpClient) {}

  getRecent(take = 10): Observable<Post[]> {
    return this.http.get<Post[]>(`${this.base}/post/recent`, { params: { take } as any });
  }

  createPost(req: { userId: string; contentText: string; mediaUrl?: string | null }): Observable<Post> {
    return this.http.post<Post>(`${this.base}/post`, req);
  }
}
