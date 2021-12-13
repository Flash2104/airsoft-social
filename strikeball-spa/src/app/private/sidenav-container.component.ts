import { NestedTreeControl } from '@angular/cdk/tree';
import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { MatTreeNestedDataSource } from '@angular/material/tree';
import { Subject, takeUntil, tap } from 'rxjs';
import { NavigationRepository } from '../shared/repository/navigation.repository';
import { INavigationItem } from '../shared/services/dto-models/navigations/navigation-data';
import { NavigationService } from '../shared/services/navigation.service';

export interface ILink {
  path: string;
  title: string;
  icon: string;
  children?: ILink[] | null;
}

@Component({
  selector: 'air-sidenav-container',
  templateUrl: './sidenav-container.component.html',
  styleUrls: ['./sidenav-container.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SideNavContainerComponent implements OnInit, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  treeControl: NestedTreeControl<INavigationItem> =
    new NestedTreeControl<INavigationItem>((node) => node.children);

  dataSource: MatTreeNestedDataSource<INavigationItem> =
    new MatTreeNestedDataSource<INavigationItem>();

  constructor(
    private _navService: NavigationService,
    private _navRepo: NavigationRepository
  ) {}

  ngOnInit(): void {
    this._navService.loadUserNavigation().subscribe();
    this._navRepo.navData$
      .pipe(
        tap((navData) => {
          if (navData != null) {
            this.dataSource.data = [...navData[0].navItems];
          }
        }),
        takeUntil(this._destroy$)
      )
      .subscribe();
  }

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }

  hasChild: (a: number, b: ILink) => boolean = (_: number, node: ILink) =>
    !!node.children && node.children.length > 0;

  onRouterLinkActive(active: boolean, node: INavigationItem): void {
    active ? this.treeControl.expand(node) : void 0;
  }
}
