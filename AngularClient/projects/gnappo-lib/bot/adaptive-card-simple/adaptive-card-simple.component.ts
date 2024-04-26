import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CardAction } from 'botframework-directlinejs';
import { AdaptiveCardSimpleModel } from '../models/adaptive-card-simple.model';
import * as _ from 'lodash';

@Component({
  selector: 'bot-adaptive-card-simple',
  templateUrl: './adaptive-card-simple.component.html',
  styleUrls: ['./adaptive-card-simple.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AdaptiveCardSimpleComponent implements OnInit {
  @Input() card?: AdaptiveCardSimpleModel
  @Output() selectSuggestion = new EventEmitter<CardAction>();
  @Output() loaded = new EventEmitter<AdaptiveCardSimpleModel>();

  innerCard?: AdaptiveCardSimpleModel;
  bodyContentsLoded: boolean[];
  constructor() { 
    this.bodyContentsLoded = [];
  }

  ngOnInit(): void {
    this.innerCard = _.cloneDeep(this.card);
    if(this.innerCard){
      this.bodyContentsLoded = this.innerCard.actions.map(() => false);
    }else{
      this.bodyContentsLoded = [];
    }
  }

  handleSuggestedAction(action: any) {
    this.selectSuggestion.emit(action);
  }

  showBodyImage(index: number, imageEl: HTMLElement){
    imageEl.hidden = false;
    this.bodyContentsLoded[index] = true;
    this.loaded.emit(_.cloneDeep(this.innerCard));
  }
}
