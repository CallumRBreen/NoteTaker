import { SignUpUser } from './../models/signUpUser';
import { LoginUser } from './../models/loginUser';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { User } from '../models/user';

@Injectable({
    providedIn: 'root',
})
export class AuthService {
    url: string = environment.apiUrl + 'users/';

    constructor(private http: HttpClient) { }

    login(loginUser: LoginUser): Observable<User> {
        return this.http.post<User>(`${this.url}login`, loginUser);
    }

    signUp(signUpUser: SignUpUser): Observable<User> {
        return this.http.post<User>(`${this.url}`, signUpUser);
    }
}
