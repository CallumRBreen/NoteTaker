import { UserService } from './../core/services/user.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material';

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

  constructor(private userService: UserService, private router: Router, private snackBar: MatSnackBar) { }

  ngOnInit() {
  }

  submit() {
    if (this.form.valid) {
      this.isSubmittedFormValid = true;
      this.userService.signUp({
        username: this.form.controls['username'].value,
        firstName: this.form.controls['firstName'].value,
        lastName: this.form.controls['lastName'].value,
        password: this.form.controls['password'].value
      }).subscribe(
        (data) => {
          this.showSuccessMessage();
          this.router.navigate(['/login']);
          this.isSignupError = false;
        },
        (error) => { this.isSignupError = true; })
    }
    else {
      this.isSubmittedFormValid = false;
    }
  }

  showSuccessMessage() {
    this.snackBar.open('User successfully created.', 'Dismiss', {
      duration: 2000
    });
  }
}
