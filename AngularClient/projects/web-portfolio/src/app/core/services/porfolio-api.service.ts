import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { SubscriptionDestroyer } from 'gnappo-lib';
import { Observable } from 'rxjs';
import { ContactMessageDto } from '../dto/request/contact-message.dto';
import { TopicInfoDto } from '../dto/response/article-info.dto';
import { ArticleDto } from '../dto/response/article.dto';
import { BlogPostDto } from '../dto/response/blog-post.dto';
import { ContentUrlDto } from '../dto/response/content-url.dto';

@Injectable({
  providedIn: 'root'
})
export class PorfolioApiService extends SubscriptionDestroyer {
  protected apiUrl: string;
  constructor(protected http: HttpClient, @Inject('WEB-PORTFOLIO-API') WEB_PORTFOLIO_API: string) { 
    super();
    this.apiUrl = WEB_PORTFOLIO_API;
  }
  protected composeUrl(url: string) {
      return `${this.apiUrl}${url}`;
  }

  getArticle(name: string): Observable<ArticleDto>{
    return this.http.get<ArticleDto>(
      this.composeUrl(`Articles/${name}`));
  }

  getBlogPost(name: string): Observable<BlogPostDto>{
    return this.http.get<BlogPostDto>(
      this.composeUrl(`Blog/${name}`));
  }

  getContentUrl(name: string): Observable<ContentUrlDto>{
    return this.http.get<ContentUrlDto>(
      this.composeUrl(`Storage/${name}`));
  }

  getMainTopics(): Observable<TopicInfoDto[]>{
    return this.http.get<TopicInfoDto[]>(
      this.composeUrl(`Topics`));
  }

  sendContactMessage(message: ContactMessageDto): Observable<void>{
    return this.http.post<void>(
      this.composeUrl(`Contact`), message);
  }
}
