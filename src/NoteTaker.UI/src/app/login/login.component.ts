import { Router } from '@angular/router';
import { UserService } from './../core/services/user.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  isSubmittedFormValid = true;
  isLoginError = false;
  form: FormGroup = new FormGroup({
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required, Validators.minLength(6), Validators.maxLength(60)])
  });
  returnUrl: string;

  constructor(private userService: UserService, private router: Router) { }

  ngOnInit() {
    this.userService.logout();
  }

  submit() {
    if (this.form.valid) {
      this.isSubmittedFormValid = true;
      var username = this.form.controls['username'].value;
      var password = this.form.controls['password'].value;
      this.userService.login({ username: username, password: password }).subscribe(
        data => {
          this.isLoginError = false;
          this.router.navigate(['/notes']);
        },
        error => {
          this.isLoginError = true;
        })
    }
    else {
      this.isSubmittedFormValid = false;
    }
  }
}
