import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { AdaptiveCardSimpleComponent } from './adaptive-card-simple/adaptive-card-simple.component';
import { ButtonModule } from 'primeng/button';
import {SkeletonModule} from 'primeng/skeleton';
import { MediaAttachmentComponent } from './media-attachment/media-attachment.component';
import { AvatarModule } from 'primeng/avatar';
import { ChatMessageComponent } from './chat-message/chat-message.component';
import { ChatSkeletonComponent } from './chat-skeleton/chat-skeleton.component';
import { GnappoLibModule } from 'gnappo-lib';

@NgModule({
  declarations: [
    AdaptiveCardSimpleComponent,
    MediaAttachmentComponent,
    ChatMessageComponent,
    ChatSkeletonComponent,
    ChatSkeletonComponent
  ],
  imports: [
    CommonModule,
    ButtonModule,
    SkeletonModule,
    AvatarModule,
    GnappoLibModule
  ],
  exports:[
    AdaptiveCardSimpleComponent,
    MediaAttachmentComponent,
    ChatMessageComponent,
    ChatSkeletonComponent
  ],
  providers:[]
})
export class BotModule { }
