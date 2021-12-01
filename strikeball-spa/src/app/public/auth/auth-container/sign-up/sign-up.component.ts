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

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SignUpComponent implements OnInit, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  form: FormGroup = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(6),
    ]),
    confirmPassword: new FormControl('', [
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

  ngOnInit(): void {}

  onSubmit(): void {}
}
