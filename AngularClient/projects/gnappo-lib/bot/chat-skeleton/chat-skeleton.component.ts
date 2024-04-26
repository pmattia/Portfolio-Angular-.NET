import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'bot-chat-skeleton',
  templateUrl: './chat-skeleton.component.html',
  styleUrls: ['./chat-skeleton.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ChatSkeletonComponent implements OnInit {
  @Input() showAvatar: boolean | undefined;
  @Input() avatarIconUrl: string | undefined;
  @Input() collapsed: boolean;

  constructor() {
    this.collapsed = false;
    this.showAvatar = false;
  }

  ngOnInit(): void {
  }

}
