import { Component } from '@angular/core';

export interface ILink {
  path: string;
  title: string;
  icon: string;
}

@Component({
  selector: 'air-sidenav-container',
  templateUrl: './sidenav-container.component.html',
  styleUrls: ['./sidenav-container.component.scss'],
  // changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SideNavContainerComponent {
  public links: ILink[] = [
    { path: '/private/profile', title: 'Профиль', icon: 'person' },
    { path: '/private/team', title: 'Команда', icon: 'groups' },
    { path: '/private/events', title: 'События', icon: 'calendar_view_month' },
  ];

  public typesOfShoes: string[] = [
    'Boots',
    'Clogs',
    'Loafers',
    'Moccasins',
    'Sneakers',
  ];

  constructor() {}
}
