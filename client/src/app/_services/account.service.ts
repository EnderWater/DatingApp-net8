import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { Login } from '../_interfaces/login';
import { User } from '../_interfaces/user';
import { map, Observable } from 'rxjs';
import { Register } from '../_interfaces/register';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private http = inject(HttpClient);
  baseUrl = 'https://localhost:5001/api/';
  currentUser = signal<User | null>(null);

  public async login(loginBody: Login) {
    const loginUrl = this.baseUrl + 'account/login';
    return this.http.post<User>(loginUrl, loginBody).pipe(
      map(user => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUser.set(user);
        }
      })
    );
  }

  public logout() {
    localStorage.removeItem('user');
    this.currentUser.set(null);
  }

  public register(registerUser: Register) {
    const registerUrl = this.baseUrl + "account/register";
    return this.http.post(registerUrl, registerUser);
  }

  public checkUserName(name: string) {
    const checkUserNameUrl = this.baseUrl + "account/check-username";
    return this.http.get<boolean>(checkUserNameUrl, { params: { "username": name } });
  }

  getUsers() {
    // Http request in Angular
    // The get request NEEDS to have a subscribe, otherwise it doesn't do anything.
    return this.http.get("https://localhost:5001/api/users");
  }
}
