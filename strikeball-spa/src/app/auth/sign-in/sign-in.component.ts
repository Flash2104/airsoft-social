import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import {ErrorStateMatcher} from '@angular/material/core';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent implements OnInit {
  emailFormControl = new FormControl('', [Validators.required, Validators.email]);
  form: FormGroup = new FormGroup({
    emailFormControl: this.emailFormControl,
    password: new FormControl('', [Validators.required, Validators.minLength(6)])
  });
  matcher = new MyErrorStateMatcher();
  constructor() { }

  ngOnInit(): void {
  }

}


export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const isSubmitted = form && form.submitted;
    return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
  }
}
