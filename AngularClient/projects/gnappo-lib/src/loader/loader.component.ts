import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'g-loader',
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LoaderComponent implements OnInit {
  @Input() visible = true;
  @Input() loadingText = '';
  @Input() zIndex: number = 9999;
  @Input() imageUrl: string | undefined
  @Input() inverseColors = false;
  
  constructor() { }

  ngOnInit(): void {
    
  }

}
