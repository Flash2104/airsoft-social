import { ITokenData } from '../token-data';
import { IUserData } from '../user-data';

export interface ISignUpResponse {
  tokenData: ITokenData | null;
  user: IUserData | null;
}
