import { createAction, props } from '@ngrx/store';
import { TopicInfoDto } from '../../dto/response/article-info.dto';

export const showErrorPage = createAction('[UI] Show error page',
props<{error: string}>() //should evovle with title and text params
);
export const showErrorPageSuccess = createAction('[UI] Show error page success',
props<{error: string}>()
);
export const showErrorPageFailure = createAction('[UI] Show error page failure',
props<{error: string}>()
);

export const greeting = createAction('[UI] Greeting');
export const greetingSuccess = createAction('[UI] Greeting success');
export const greetingFailure = createAction('[UI] Greeting failure',
    props<{error: string}>()
);


export const conversate = createAction('[UI] Conversate');
export const conversateSuccess = createAction('[UI] Conversate success'
);
export const conversateFailure = createAction('[UI] Conversate failure',
    props<{error: string}>()
);

export const showArticle = createAction('[UI] Show Article',
    props<{articleName: string}>());
export const showArticleSuccess = createAction('[UI] Show Article success'
);
export const showArticleFailure = createAction('[UI] Show Article failure',
    props<{error: string}>()
);

export const showBlogPost = createAction('[UI] Show Blog Post',
    props<{blogPostName: string}>());
export const showBlogPostSuccess = createAction('[UI] Show Blog Post success'
);
export const showBlogPostFailure = createAction('[UI] Show Blog Post failure',
    props<{error: string}>()
);

export const showPdf = createAction('[UI] Show Pdf',
    props<{pdfName: string}>());
export const showPdfSuccess = createAction('[UI] Show Pdf success'
);
export const showPdfFailure = createAction('[UI] Show Pdf failure',
    props<{error: string}>()
);

export const contactMe = createAction('[UI] Contact Me');
export const contactMeSuccess = createAction('[UI] Contact Me success');
export const contactMeFailure = createAction('[UI] Contact Me failure',
    props<{error: string}>()
);

export const goHome = createAction('[UI] Home');
export const goHomeSuccess = createAction('[UI] Home success'
);
export const goHomeFailure = createAction('[UI] Home failure',
    props<{error: string}>()
);

export const getMainTopics = createAction('[UI] Get Main topics');
export const getMainTopicsSuccess = createAction('[UI] Get Main topics success',
    props<{ articles: TopicInfoDto[] }>()
);
export const getMainTopicsFailure = createAction('[UI] Get Main topics failure',
    props<{ error: string }>()
);

export const openSidePanel = createAction('[UI] Show sidepanel');
export const setMobile = createAction('[UI] Set Mobile',
props<{ isMobile: boolean }>()
);