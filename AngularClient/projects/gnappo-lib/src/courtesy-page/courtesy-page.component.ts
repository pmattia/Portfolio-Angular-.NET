import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { CourtesyPageSeverity } from './courtesy-page-severity.type';

@Component({
  selector: 'g-courtesy-page',
  templateUrl: './courtesy-page.component.html',
  styleUrls: ['./courtesy-page.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CourtesyPageComponent implements OnInit {
  @Input() title!: string;
  @Input() text?: string;
  @Input() severity?: CourtesyPageSeverity
  @Input() actions?: { label: string, action: () => void }[];
  @Input() showCloseButton = true;
  @Input() inverseColors = false;
  @Input() noActionPriority = false;
  @Output() onHide = new EventEmitter<void>();
  
  constructor() { 
  }

  ngOnInit(): void {
  }

  closeContentPage(){
    this.onHide.emit();
  }
}
