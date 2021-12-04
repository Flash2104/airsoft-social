import { ChangeDetectionStrategy, Component } from '@angular/core';

export interface ILink {
  path: string;
  title: string;
  icon: string;
}

@Component({
  selector: 'air-main-container',
  templateUrl: './main-container.component.html',
  styleUrls: ['./main-container.component.scss'],
  // changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MainContainerComponent {
  public links: ILink[] = [
    {path: '/private/profile', title: 'Профиль', icon: 'person'},
    {path: '/private/command', title: 'Команда', icon: 'groups'},
    {path: '/private/events', title: 'События', icon: 'calendar_view_month'}
  ];

  public typesOfShoes: string[] = ['Boots', 'Clogs', 'Loafers', 'Moccasins', 'Sneakers'];

  constructor() {}
}
