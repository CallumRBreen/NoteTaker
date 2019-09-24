import { SignUpUser } from './../models/signUpUser';
import { LoginUser } from './../models/loginUser';
import { Observable, BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { User } from '../models/user';
import { map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root',
})
export class UserService {
    url: string = environment.apiUrl + 'users/';

    private isUserLoggedIn: BehaviorSubject<boolean>;

    constructor(private http: HttpClient) {
        if (localStorage.getItem('currentUser')) {
            this.isUserLoggedIn = new BehaviorSubject<boolean>(true);
        }
        else {
            this.isUserLoggedIn = new BehaviorSubject<boolean>(false);
        }
    }

    login(loginUser: LoginUser): Observable<User> {
        return this.http.post<User>(`${this.url}login`, loginUser).pipe(map(user => {
            if (user && user.token) {
                localStorage.setItem('currentUser', JSON.stringify(user));
                this.isUserLoggedIn.next(true);
            }
            return user;
        }));

    }

    signUp(signUpUser: SignUpUser): Observable<User> {
        return this.http.post<User>(`${this.url}`, signUpUser);
    }

    logout() {
        localStorage.removeItem('currentUser');
        this.isUserLoggedIn.next(false);
    };

    get isLoggedin() {
        return this.isUserLoggedIn.asObservable();
    }
}
