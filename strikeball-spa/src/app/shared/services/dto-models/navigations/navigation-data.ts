import { IReferenceData } from './../reference-data';

export interface INavigationItem {
  id: number;
  path: string;
  title: string;
  icon: string | null | undefined;
  children?: INavigationItem[] | null;
}

export interface IRolesNavigationData {
  role: IReferenceData<number>;
  navItems: INavigationItem[];
}

export interface INavigationDataResponse {
  data: IRolesNavigationData[] | null | undefined;
}
