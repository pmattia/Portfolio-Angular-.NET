import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { GnappoLibModule } from 'gnappo-lib';
import { BotModule } from 'gnappo-lib/bot';
import { GnappoPdfViewerModule } from 'gnappo-lib/pdf-viewer';
import { AvatarModule } from 'primeng/avatar';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { SkeletonModule } from 'primeng/skeleton';
import { contentNameParam, routeArticleUrl, routeBlogUrl, routeContactsUrl, routePdfUrl } from '../../core/contants';
import { SharedModule } from '../../shared/shared.module';
import { ChatComponent } from './chat/chat.component';
import { ContentPageComponent } from './content-page/content-page.component';
import { PdfPageComponent } from './pdf-page/pdf-page.component';
import { ArticleResolver } from './resolvers/article.resolver';
import { BlogPostResolver } from './resolvers/blog-post.resolver';
import { PdfResolver } from './resolvers/pdf.resolver';
import { ContentEffects } from './store/content/content.effects';
import { contentReducer } from './store/content/content.reducers';
import { TagModule } from 'primeng/tag';

@NgModule({
  declarations: [
    ChatComponent,
    ContentPageComponent,
    PdfPageComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    InputTextModule,
    ButtonModule,
    BotModule,
    GnappoPdfViewerModule,
    GnappoLibModule,
    AvatarModule,
    SharedModule,
    SkeletonModule,
    TagModule,
    StoreModule.forFeature('conversation', contentReducer),
    EffectsModule.forFeature([ContentEffects]),
    RouterModule.forChild([
      {
        path: '',
        component: ChatComponent,
        // canActivate: [CanActivateBot],
        children: [{
          path: routeContactsUrl,
          loadChildren: () => import('./../contacts/contacts.module').then(m => m.ContactsModule),
          outlet: 'asidepanel',
        },
        {
          path: `${routePdfUrl}/:${contentNameParam}`,
          component: PdfPageComponent,
          outlet: 'asidepanel',
          resolve: { contentModel: PdfResolver }
        },
        {
          path: `${routeBlogUrl}/:${contentNameParam}`,
          component: ContentPageComponent,
          outlet: 'asidepanel',
          resolve: { contentModel: BlogPostResolver },
        },
        {
          path: `${routeArticleUrl}/:${contentNameParam}`,
          component: ContentPageComponent,
          outlet: 'asidepanel',
          resolve: { contentModel: ArticleResolver },
        }]
      }
    ])
  ],
  providers: [ContentEffects],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class ConversationModule { }
