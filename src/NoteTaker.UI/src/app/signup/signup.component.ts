import { AuthService } from './../core/services/auth.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {
  isSubmittedFormValid = true;
  isSignupError = false;
  form: FormGroup = new FormGroup({
    username: new FormControl('', [Validators.required]),
    firstName: new FormControl('', [Validators.required]),
    lastName: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required, Validators.minLength(6), Validators.maxLength(60)])
  });

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit() {
  }

  submit() {
    if (this.form.valid) {
      this.isSubmittedFormValid = true;
      this.authService.signUp({
        username: this.form.controls['username'].value,
        firstName: this.form.controls['firstName'].value,
        lastName: this.form.controls['lastName'].value,
        password: this.form.controls['password'].value
      }).subscribe(
        (data) => { this.router.navigate(['/login']); this.isSignupError = false },
        (error) => { this.isSignupError = true; })
    }
    else {
      this.isSubmittedFormValid = false;
    }
  }

}
