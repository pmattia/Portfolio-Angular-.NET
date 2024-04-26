import { Component, ElementRef, Inject, ViewChild } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { ActivityModel, CardAction, CardActionTypes, UserMessageCardAction, UserMessagesModel } from 'gnappo-lib/bot';
import * as _ from 'lodash';
import { Observable, tap } from 'rxjs';
import { USERID } from '../../../core/contants';
import { ContenType } from '../../../core/dto/enums/content-type.enum';
import { contactMe, showArticle, showBlogPost, showPdf } from '../../../core/store/actions/ui.actions';
import { AppState } from '../../../core/store/models/app-state.model';
import { UiState } from '../../../core/store/models/ui-state.model';
import { isMobileSelector, uiSelector } from '../../../core/store/selectors';
import { BotComponent } from '../../../shared/bot/bot-component';
import { sendMessage } from '../../../shared/bot/store/bot.actions';
import { botActivitiesSelector, botIsTypingSelector, botSentMessagesSelector } from '../../../shared/bot/store/selectors';
import { ScrollToBottomDirective } from '../../../shared/directives/scroll-to-bottom.directive';
import { isContentLoadingSelector } from '../store/selectors';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent extends BotComponent {
  messages: UserMessagesModel[];
  ui$: Observable<UiState>;
  userMessage: string;
  isMobile = true;
  isBotTyping = false;
  isLastMessageFromBot = true;
  lastAvatarIconUrl: string | undefined;
  isContentLoading$: Observable<boolean>;
  openUrlActions = new Map<ContenType, (action: CardAction) => void>();
  defaultAction = (action: CardAction) => {};
  @ViewChild(ScrollToBottomDirective) chatScroller!: ScrollToBottomDirective;
  @ViewChild('scrollercontainer') chatScrollerContainer!: ElementRef;

  constructor(store: Store<AppState>, @Inject(USERID) private userId: string) {
    super(store);
    
    this.initUserActionsHandlers();
    
    this.messages = [];
    this.userMessage = '';
    this.isContentLoading$ = this.store.pipe(select(isContentLoadingSelector));

    this.ui$ = this.store.pipe(select(uiSelector))
      .pipe(tap(() => this.chatScroller?.scrollToBottom(800)));

    this.initBot(userId);

    this.add$(this.store.pipe(
      select(botActivitiesSelector))
      .subscribe(activities => {
        this.messages = this.updateMessages(this.messages, activities);
      })
    );

    this.add$(this.store.pipe(
      select(botSentMessagesSelector))
      .subscribe(activities => {
        this.messages = this.removeSuggestions(this.messages);
      })
    );

    this.add$(this.store.pipe(
      select(botIsTypingSelector))
      .subscribe(isTyping => {
        this.isBotTyping = isTyping;
      })
    );

    this.add$(this.store.pipe(
      select(isMobileSelector))
      .subscribe(isMobile => {
        this.isMobile = isMobile;
      })
    );
  }

  handleSuggestedAction(action: CardAction) {
    
    this.fixScrollContainerHeight();
    
    switch (action.type as CardActionTypes) {
      case 'openUrl':
        if(!!action.value){
          const openUrlAction = this.openUrlActions.get(action.value.contentType) ?? this.defaultAction;
          openUrlAction.call(this, action);
        }
        break;
      case 'messageBack':
        this.defaultAction(action);
        break;
    }
  }

  sendMessage() {
    this.store.dispatch(sendMessage({ message: this.userMessage }));
    this.userMessage = '';
    
  }

  chatContentLoaded(event: any) {
   
    //trigger scroller adjustment
    //cloud be optimized to only trigger when the last message is from the bot
    _.debounce(() => {
      this.autoFitScrollContainerHeight();
      this.chatScroller.scrollToBottom();
    }, 500).call(this);
  }

  private initUserActionsHandlers(){
    this.openUrlActions.set(ContenType.Article, (action) => this.store.dispatch(showArticle({ articleName: action.value.contentName })));
    this.openUrlActions.set(ContenType.Blog, (action) => this.store.dispatch(showBlogPost({ blogPostName: action.value.contentName })));
    this.openUrlActions.set(ContenType.Pdf, (action) => this.store.dispatch(showPdf({ pdfName: action.value.contentName })));
    this.openUrlActions.set(ContenType.Contact, (action) => this.store.dispatch(contactMe()));
    this.openUrlActions.set(ContenType.ContentUrl, (action) => window.open(action.url));
    this.defaultAction = (action) => this.store.dispatch(sendMessage({ message: action.text!, value: action.value }));
  }

  //to prevent the chat scroller from jumping when the height of the chat container changes
  private fixScrollContainerHeight() {
    this.chatScrollerContainer.nativeElement.style.minHeight  = this.chatScrollerContainer.nativeElement.clientHeight + 'px';
  }

  //to let the chat scroller adjust to the new height of the chat container
  private autoFitScrollContainerHeight() {
    this.chatScrollerContainer.nativeElement.style.minHeight  = 'auto';
  }

  private removeSuggestions(messages: UserMessagesModel[]): UserMessagesModel[] {
    messages = this.messages.map(m => {
      m.activities = m.activities.map(a => {
        a.suggestedActions?.actions.forEach(action => {
          action.disabled = true;
        });
        return { ...a };
      });
      return { ...m };
    });

    return [...messages];
  }

  private updateMessages(messages: UserMessagesModel[], activities: ActivityModel[]): UserMessagesModel[] {
    let lastUser: string | undefined;

    activities.forEach(activity => {
      const isAddedToMessages = _.flattenDeep(messages.map(m => m.activities)).some(a => a.id === activity.id);
      if (!isAddedToMessages) {
        const lastMessage = _.last(messages);
        if (!lastMessage
          || activity.from.id !== lastMessage.from.id
          || _.last(lastMessage.activities)?.attachments?.length! > 0
        ) {
          messages.push({
            from: activity.from,
            activities: [this.mapToActivity(activity)]
          });
          lastUser = activity.from.id;
        } else {
          if (!lastMessage.from.avatar && activity.from.avatar) {
            lastMessage.from = activity.from
          }
          lastMessage.activities.push(
            this.mapToActivity(activity)
          )
          messages[messages.length - 1] = { ...lastMessage };
        }
      }
    });

    this.isLastMessageFromBot = lastUser !== this.userId;
    const lastBotMessages = _.last(messages.filter(a => a.from.isBot));
    this.lastAvatarIconUrl = lastBotMessages?.from.iconUrl;

    return messages;
  }

  private mapToActivity(activity: ActivityModel | any) {
    return {
      id: activity.id!,
      timestamp: new Date(activity.timestamp!),
      text: activity.text,
      suggestedActions: {
        actions: this.mapToCardAction(activity.suggestedActions?.actions),
        to: activity.suggestedActions?.to
      },
      attachments: _.cloneDeep(activity.attachments)
    };
  }

  private mapToCardAction(suggestedActions: CardAction[]): UserMessageCardAction[] {
    if (!suggestedActions) {
      return [];
    }
    return suggestedActions.map(action => {
      return {
        displayText: action.displayText,
        image: action.image,
        text: action.text!,
        title: action.title,
        type: action.type,
        value: action.value,
        url: action.url,
        disabled: false
      }
    });
  }
}
