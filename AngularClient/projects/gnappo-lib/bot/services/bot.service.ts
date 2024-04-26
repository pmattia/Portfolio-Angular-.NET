import { Inject, Injectable } from '@angular/core';
import { AdaptiveCard, DirectLine, Message, UnknownMedia } from 'botframework-directlinejs';
import { BOT_API_SECRET, SubscriptionDestroyer } from 'gnappo-lib';
import { Observable, Subject, distinctUntilChanged } from 'rxjs';
import { ActivityModel, Attachment, AttachmentContentType, BotAttachmentType, ConnectionStatus, Conversation } from '../models/bot.interfaces';

@Injectable({
  providedIn: 'root'
})
export class BotService extends SubscriptionDestroyer {

  private directLine: DirectLine;
  private activitiesSubject$: Subject<ActivityModel>;
  private connectionStatusSubject$: Subject<ConnectionStatus>;
  private postMessageSubject$: Subject<string>;
  private userId: string | undefined;
  private conversation: Conversation | undefined;
  private connectionStatus: ConnectionStatus;
  private isTypingSubject$: Subject<boolean>;

  constructor(@Inject(BOT_API_SECRET) private DIRECTLINE_SECRET: string) {
    super();
    this.connectionStatus = ConnectionStatus.Uninitialized;
    this.activitiesSubject$ = new Subject();
    this.connectionStatusSubject$ = new Subject();
    this.postMessageSubject$ = new Subject();
    this.isTypingSubject$ = new Subject();
    this.directLine = this.createDirectLine();

  }

  restartConversation(): Observable<ConnectionStatus> {
    // this.directLine.reconnect(this.conversation!);
    this.directLine = this.createDirectLine();

    this.initSubjects(this.directLine);

    this.wakeup();

    return this.connectionStatusSubject$.asObservable();
  }

  startConversation(userId: string): Observable<ConnectionStatus> {

    this.userId = userId;
    this.directLine.setUserId(userId);

    this.initSubjects(this.directLine);

    return this.connectionStatusSubject$.asObservable();
  }

  private wakeup() {
    this.directLine.postActivity({
      from: { id: this.userId! }, // required (from.name is optional)
      type: 'event',
      name: 'start',
      value: 0
    }).subscribe(
      id => {
        const activityId = `Posted activity, assigned ID ${id}`;
        this.postMessageSubject$.next(activityId);
      },
      error => console.error("Error posting activity", error)
    );
  }

  private createDirectLine() {
    return new DirectLine({
      secret: this.DIRECTLINE_SECRET //'[secret]',
      /*token: or put your Direct Line token here (supply secret OR token, not both) */
      /*domain: optional: if you are not using the default Direct Line endpoint, e.g. if you are using a region-specific endpoint, put its full URL here */
      /*webSocket: optional: false if you want to use polling GET to receive messages. Defaults to true (use WebSocket). */
      /*pollingInterval: optional: set polling interval in milliseconds. Defaults to 1000 */
      /*timeout: optional: a timeout in milliseconds for requests to the bot. Defaults to 20000 */
      /*conversationStartProperties: {optional: properties to send to the bot on conversation start 
          locale: 'en-US'
      }*/
    });
  }

  private initSubjects(directLine: DirectLine) {
    directLine.activity$
      .subscribe(
        activity => {
          const botAttachments: Attachment[] = [];
          const message = activity as Message;
          if (message && message.attachments && message.attachments.length > 0) {
            message.attachments.forEach(attachment => {
              if (attachment.contentType === "application/vnd.microsoft.card.adaptive") {
                const card = (attachment as AdaptiveCard);
                if (!!card.content.actions) {
                  card.content.actions.forEach((action: any) => {
                    switch (action.type) {
                      case 'Action.OpenUrl':
                        action.type = 'openUrl';
                        break;
                      case 'openUrl':
                        break;
                      case 'messageBack':
                        break
                      case 'Action.Submit':
                        action.type = 'messageBack';
                        action.text = action.data;
                        break;
                    }
                  });
                }

                botAttachments.push(
                  {
                    type: BotAttachmentType.AdaptiveCard,
                    adaptiveCard: card.content,
                    contentType: card.contentType
                  }
                );
              } else {
                const content = attachment as UnknownMedia;
                botAttachments.push(
                  {
                    type: BotAttachmentType.ContentUrl,
                    contentUrl: content.contentUrl,
                    contentType: content.contentType as AttachmentContentType
                  }
                );
              }
            });

          }
          if (activity.type === 'message') {
            const model = activity as ActivityModel;
            let isBot = model.from.id !== this.userId;

            if (!isBot || this.getBoolean(model.channelData?.isTyping)) {
              this.isTypingSubject$.next(true);
            } else {
              this.isTypingSubject$.next(false);
            }
            model.from.isBot = isBot;
            model.from.avatar = model.channelData?.avatar;
            model.from.iconUrl = model.channelData?.avatarUrl;
            model.timestamp = new Date(activity.timestamp!);
            model.attachments = botAttachments;
            this.conversation = model.conversation;
            this.activitiesSubject$.next(model);
          }
        }
      );

    directLine.connectionStatus$
      .subscribe(connectionStatus => {
        switch (connectionStatus) {
          case ConnectionStatus.Uninitialized:    // the status when the DirectLine object is first created/constructed
            break;
          case ConnectionStatus.Connecting:       // currently trying to connect to the conversation
            break;
          case ConnectionStatus.Online:           // successfully connected to the converstaion. Connection is healthy so far as we know.
            break;
          case ConnectionStatus.ExpiredToken:     // last operation errored out with an expired token. Your app should supply a new one.
            break;
          case ConnectionStatus.FailedToConnect:  // the initial attempt to connect to the conversation failed. No recovery possible.
            break;
          case ConnectionStatus.Ended:            // the bot ended the conversation
            break;
        }
        this.connectionStatus = connectionStatus;
        this.connectionStatusSubject$.next(connectionStatus);
      });
  }

  private getBoolean(value: any): boolean {
    switch (value) {
      case true:
      case "true":
      case "True":
      case 1:
      case "1":
      case "on":
      case "yes":
        return true;
      default:
        return false;
    }
  }

  get activity$(): Observable<ActivityModel> {
    return this.activitiesSubject$.asObservable();
  }

  get isTyping$(): Observable<boolean> {
    return this.isTypingSubject$.asObservable().pipe(
      distinctUntilChanged()
    );
  }

  postMessage(message: string, value: any = null): Observable<string> {
    this.directLine.postActivity({
      from: { id: this.userId! }, // required (from.name is optional)
      type: 'message',
      text: message,
      value: value
    }).subscribe(
      id => {
        const activityId = `Posted activity, assigned ID ${id}`;
        this.postMessageSubject$.next(activityId);
      },
      error => {
        console.error("Error posting activity", error);
      }
    );

    return this.postMessageSubject$.asObservable();
  }
}
