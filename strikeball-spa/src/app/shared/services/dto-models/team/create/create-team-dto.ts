import { ITeamData } from '../team-data';

export interface ICreateTeamRequest {
  city: string | null | undefined;
  title: string;
  foundationDate: string | null | undefined;
  avatar: string | null | undefined;
}

export interface ICreateTeamResponse {
  teamData: ITeamData | null;
}
