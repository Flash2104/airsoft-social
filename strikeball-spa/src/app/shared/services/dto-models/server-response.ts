export type OperationCode =
  | 'Success'
  | 'CallFailed'
  | 'OperationFailed'
  | 'IncorrentConditions'
  | 'AssertFailed'
  | 'UnAuthorized';

export interface IServerError {
  code: number;
  message: string;
  reason: string;
}

export interface IOperationStatus {
  code: OperationCode | null;
  error: IServerError;
}

export interface IServerResponse<T> {
  result: T | null;
  totalCount?: number | null;
  status: IOperationStatus;
}
