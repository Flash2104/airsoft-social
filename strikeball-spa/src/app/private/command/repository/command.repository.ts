import { Injectable } from '@angular/core';
import { createState, select, Store, withProps } from '@ngneat/elf';
import { Observable } from 'rxjs';
import { v1 as uuidv1 } from 'uuid';

export interface ITeamState {
  team: unknown;
  loading: boolean;
}

@Injectable()
export class TeamRepository {
  _state: {
    state: ITeamState;
    config: undefined;
  } = createState(
    withProps<ITeamState>({
      team: null,
      loading: false,
    })
  );

  _name: string = `team-${uuidv1().substr(-8)}`;

  teamStore: Store<
    { state: ITeamState; name: string; config: undefined },
    ITeamState
  > = new Store({
    state: this._state.state,
    name: this._name,
    config: this._state.config,
  });

  team$: Observable<unknown | null> = this.teamStore.pipe(
    select((st) => st.team)
  );

  loading$: Observable<boolean> = this.teamStore.pipe(
    select((st) => st.loading)
  );

  setLoading(loading: ITeamState['loading']): void {
    this.teamStore.update((state) => ({
      ...state,
      loading,
    }));
  }

  setProfile(team: ITeamState['team']): void {
    this.teamStore.update((st) => ({
      ...st,
      team,
    }));
  }

  destroy(): void {
    this.teamStore.complete();
    this.teamStore.destroy();
  }
}
