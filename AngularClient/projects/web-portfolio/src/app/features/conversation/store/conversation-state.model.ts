import { ArticleDto } from "../../../core/dto/response/article.dto";
import { BlogPostDto } from "../../../core/dto/response/blog-post.dto";
import { ContentUrlDto } from "../../../core/dto/response/content-url.dto";

export interface ConversationState{
    isLoading: boolean;
    lastArticle?: ArticleDto;
    lastBlogPost?: BlogPostDto;
    lastContentUrl?: ContentUrlDto;
}
