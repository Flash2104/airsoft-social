import { Injectable, OnDestroy } from '@angular/core';
import { createState, select, Store, withProps } from '@ngneat/elf';
import { selectAll, upsertEntities, withEntities } from '@ngneat/elf-entities';
import { Observable } from 'rxjs';
import { v1 as uuidv1 } from 'uuid';
import { ICitiesData } from '../services/dto-models/references/cities/cities-dto';

@Injectable()
export class CitiesRepository implements OnDestroy {
  _state: {
    state: { entities: Record<number, ICitiesData> };
    config: { idKey: 'id' };
  } = createState(
    withEntities<ICitiesData>({ initialValue: [], idKey: 'id' }),
    withProps<{ loading: boolean }>({ loading: false })
  );

  _name: string = `cities-${uuidv1().substring(0, 8)}`;

  citiesStore: Store<{
    state: {
      entities: Record<number, ICitiesData>;
      ids: number[];
      loading: boolean;
    };
    name: string;
    config: undefined;
  }> = new Store({
    state: this._state.state,
    name: this._name,
    config: this._state.config,
  });

  cities$: Observable<ICitiesData[] | null> = this.citiesStore.pipe(
    selectAll()
  );

  loading$: Observable<boolean> = this.citiesStore.pipe(
    select((st) => st.loading)
  );

  setLoading(loading: boolean): void {
    this.citiesStore.update((state) => ({
      ...state,
      loading,
    }));
  }

  upsertCities(cities: ICitiesData[] | null): void {
    if (cities != null) {
      this.citiesStore.update(upsertEntities(cities));
    }
  }

  ngOnDestroy(): void {
    this.citiesStore.complete();
    this.citiesStore.destroy();
  }
}
