import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Subject } from 'rxjs';
import { FormErrorStateMatcher } from '../../../../shared/utils/error-state-matcher';

export interface ISignInData {
  login: string;
  password: string;
}

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SignInComponent implements OnInit, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  form: FormGroup = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(6),
    ]),
  });

  matcher: FormErrorStateMatcher = new FormErrorStateMatcher();

  constructor(public dialog: MatDialog) {}

  ngOnDestroy(): void {
    this._destroy$.next();
    this._destroy$.complete();
  }

  ngOnInit(): void {
    console.log('Init');
  }

  onSubmit(): void {
    console.log('Submit');
  }
}
