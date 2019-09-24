import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { UserService } from './core/services/user.service';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  isUserLoggedIn$: Observable<boolean>;

  constructor(private userService: UserService, private router: Router) {
  }

  ngOnInit(): void {
    this.isUserLoggedIn$ = this.userService.isLoggedin;
  }

  logout() {
    this.userService.logout();
    this.router.navigate(['/login']);
  }
}
