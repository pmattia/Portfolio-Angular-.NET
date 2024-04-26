import { ChangeDetectionStrategy, Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import * as _ from 'lodash';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'g-burger',
  templateUrl: './burger.component.html',
  styleUrls: ['./burger.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BurgerComponent implements OnInit, OnChanges {
  megamenuVisibility: boolean;
  @Input() darkMode: boolean;
  @Input() fullScreen: boolean;
  @Input() model: {
          label?: string;
          icon?: string;
          command?: (event?: any) => void;
        }[]
  innerModel: {
    label?: string;
    icon?: string;
    command?: (event?: any) => void;
  }[]

  constructor() {
    this.fullScreen = true;
    this.darkMode = true;
    this.model = [];
    this.innerModel = [];
    this.megamenuVisibility = false;
  }

  ngOnInit(): void {
    this.innerModel = _.cloneDeep(this.model);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['model']) {
      this.innerModel = _.cloneDeep(this.model);
    }
  }

  toggleMegamenu(){
    this.megamenuVisibility = !this.megamenuVisibility;
  }

  openLink(item: MenuItem){
    if(item.command){
      item.command();
      this.toggleMegamenu();
    }
  }
}
