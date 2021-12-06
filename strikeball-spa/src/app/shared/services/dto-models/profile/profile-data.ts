import { IReferenceData } from './../reference-data';
export interface IProfileData {
  id: string | null | undefined;
  name: string | null | undefined;
  surname: string | null | undefined;
  avatarData: string | null | undefined;
  email: string | null | undefined;
  phone: string | null | undefined;
  teamId: string | null | undefined;
  roles: IReferenceData<number>[] | null | undefined;
}
