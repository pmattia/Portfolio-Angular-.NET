import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Attachment } from '../models/bot.interfaces';
import * as _ from 'lodash';

@Component({
  selector: 'bot-media-attachment',
  styleUrls: ['./media-attachment.component.scss'],
  templateUrl: './media-attachment.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MediaAttachmentComponent implements OnInit {
  @Input() attachment?: Attachment;
  @Output() loaded = new EventEmitter<Attachment>();

  innerAttachment?: Attachment;
  contentLoded: boolean;
  constructor() { 
    this.contentLoded = false;
  }

  ngOnInit(): void {
    this.innerAttachment = _.cloneDeep(this.attachment);
  }

  showBodyImage(imageEl: HTMLElement){
    imageEl.hidden = false;
    this.contentLoded = true;
    this.loaded.emit(_.cloneDeep(this.innerAttachment));
  }
}
