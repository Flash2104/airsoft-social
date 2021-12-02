import { ITokenData } from '../token-data';
import { IUserData } from '../user-data';

export interface ISignInResponse {
  tokenData: ITokenData | null;
  user: IUserData | null;
}
