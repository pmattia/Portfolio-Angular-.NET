import { createAction, props } from '@ngrx/store';
import { ArticleDto } from 'projects/web-portfolio/src/app/core/dto/response/article.dto';
import { BlogPostDto } from 'projects/web-portfolio/src/app/core/dto/response/blog-post.dto';
import { ContentUrlDto } from 'projects/web-portfolio/src/app/core/dto/response/content-url.dto';


export const getArticle = createAction('[CONTENT] Get Article ',
    props<{ name: string }>()
);
export const getArticleSuccess = createAction('[CONTENT] Get Article success',
    props<{ article: ArticleDto }>()
);
export const getArticleFailure = createAction('[CONTENT] Get Article failure',
    props<{ error: string }>()
);


export const getBlogPost = createAction('[CONTENT] Get Blog Post',
    props<{ name: string }>()
);
export const getBlogPostSuccess = createAction('[CONTENT] Get Blog Post success',
    props<{ blogPost: BlogPostDto }>()
);
export const getBlogPostFailure = createAction('[CONTENT] Get Blog Post failure',
    props<{ error: string }>()
);


export const getPdf = createAction('[CONTENT] Get Pdf',
    props<{ name: string }>()
);
export const getPdfSuccess = createAction('[CONTENT] Get Pdf success',
    props<{ pdf: ContentUrlDto }>()
);
export const getPdfFailure = createAction('[CONTENT] Get Pdf failure',
    props<{ error: string }>()
);