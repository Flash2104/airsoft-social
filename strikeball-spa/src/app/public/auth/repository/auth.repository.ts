import { Injectable } from '@angular/core';
import { createState, select, Store, withProps } from '@ngneat/elf';
import { localStorageStrategy, persistState } from '@ngneat/elf-persist-state';
import { Observable } from 'rxjs';

export interface IAuthUser {
  id: string;
  name: string | null;
}

export interface IAuthToken {
  jwt: string | null;
  expiryDate: string | null;
}

export interface IAuthState {
  user: IAuthUser | null;
  token: IAuthToken | null;
  loading: boolean;
}

const { state, config } = createState(
  withProps<IAuthState>({ user: null, token: null, loading: false })
);

const name = 'auth';

const authStore = new Store({ state, name, config });

export const authPersist = persistState(authStore, {
  key: name,
  storage: localStorageStrategy,
});

@Injectable({ providedIn: 'root' })
export class AuthRepository {
  user$: Observable<IAuthUser | null> = authStore.pipe(
    select((state) => state.user)
  );

  token$: Observable<IAuthToken | null> = authStore.pipe(
    select((state) => state.token)
  );

  loading$: Observable<boolean> = authStore.pipe(
    select((state) => state.loading)
  );

  setLoading(loading: IAuthState['loading']): void {
    authStore.update((state) => ({
      ...state,
      loading,
    }));
  }

  updateUser(user: IAuthState['user']): void {
    authStore.update((state) => ({
      ...state,
      user,
    }));
  }

  updateToken(token: IAuthState['token']): void {
    authStore.update((state) => ({
      ...state,
      token,
    }));
  }
}
