import { Injectable } from '@angular/core';
import { createState, select, Store, withProps } from '@ngneat/elf';
import { Observable } from 'rxjs';
import { v1 as uuidv1 } from 'uuid';
import { IProfileData } from '../../../shared/services/dto-models/profile/profile-data';

export interface IProfileState {
  profile: IProfileData | null;
  loading: boolean;
}

@Injectable()
export class ProfileRepository {
  _state: {
    state: IProfileState;
    config: undefined;
  } = createState(
    withProps<IProfileState>({
      profile: null,
      loading: false,
    })
  );

  _name: string = `profile-${uuidv1().substr(-8)}`;

  profileStore: Store<
    { state: IProfileState; name: string; config: undefined },
    IProfileState
  > = new Store({
    state: this._state.state,
    name: this._name,
    config: this._state.config,
  });

  profile$: Observable<IProfileData | null> = this.profileStore.pipe(
    select((st) => st.profile)
  );

  loading$: Observable<boolean> = this.profileStore.pipe(
    select((st) => st.loading)
  );

  setLoading(loading: IProfileState['loading']): void {
    this.profileStore.update((state) => ({
      ...state,
      loading,
    }));
  }

  setProfile(profile: IProfileState['profile']): void {
    this.profileStore.update((st) => ({
      ...st,
      profile,
    }));
  }

  destroy(): void {
    this.profileStore.complete();
    this.profileStore.destroy();
  }
}
