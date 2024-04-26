import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'g-pdf-viewer',
  templateUrl: './pdf-viewer.component.html',
  styleUrls: ['./pdf-viewer.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PdfViewerComponent implements OnInit {
  @Input() src: string = '';
  innerSrc: string ='';
  constructor() { }

  ngOnInit(): void {
    this.innerSrc = this.src;
  }

}
