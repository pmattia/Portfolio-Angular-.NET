import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { BotAttachmentType, CardAction } from './../models/bot.interfaces';
import * as _ from 'lodash';
import { UserMessagesModel } from '../models/user-messages.model';

@Component({
  selector: 'bot-chat-message',
  templateUrl: './chat-message.component.html',
  styleUrls: ['./chat-message.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ChatMessageComponent implements OnInit {

  @Input() last: boolean | undefined;
  @Input() message: UserMessagesModel | undefined;
  @Input() collapsed: boolean;
  @Output() selectSuggestion = new EventEmitter<CardAction>();
  @Output() contentLoaded = new EventEmitter<void>();
  BotAttachmentType = BotAttachmentType;
  innerMessage: UserMessagesModel | undefined;
  constructor() {
    this.collapsed = false;
  }

  ngOnInit(): void {
    this.innerMessage = _.cloneDeep(this.message);
  }

  handleSuggestedAction(action: any) {
    this.selectSuggestion.emit(action);
  }

  emitContentLoaded(){
    this.contentLoaded.emit();
  }
}
