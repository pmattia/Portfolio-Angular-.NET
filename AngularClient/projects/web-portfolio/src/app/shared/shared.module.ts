import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { AutoFocusModule } from 'primeng/autofocus';
import { AvatarModule } from 'primeng/avatar';
import { ButtonModule } from 'primeng/button';
import { BotEffects } from './bot/store/bot.effects';
import { botReducer } from './bot/store/bot.reducers';
import { AutoHeightDirective } from './directives/auto-height.directive';
import { ConversateOnBackDirective } from './directives/conversate-on-back.directive';
import { ScrollToBottomDirective } from './directives/scroll-to-bottom.directive';

@NgModule({
  declarations: [
    ScrollToBottomDirective,
    AutoHeightDirective,
    ConversateOnBackDirective
  ],
  imports: [
    CommonModule,
    FormsModule,
    ButtonModule,
    AvatarModule,
    StoreModule.forFeature('bot', botReducer),
    EffectsModule.forFeature([BotEffects])
  ],
  exports: [
    ScrollToBottomDirective,
    AutoFocusModule,
    AutoHeightDirective,
    ConversateOnBackDirective
  ],
  providers: [BotEffects]
})
export class SharedModule { }
