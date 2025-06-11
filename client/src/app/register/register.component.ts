import { Component, inject, input, OnInit, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { Register } from '../_interfaces/register';
import { createDefaultRegister } from '../_interfaces/factory';
import { User } from '../_interfaces/user';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit {
  public accountService = inject(AccountService);
  public registerUser: Register = createDefaultRegister();
  public usernameExists?: boolean = undefined;
  public existingUsers: any[] = [];
  public cancelRegister = output<boolean>();

  ngOnInit() {
    this.getUsers();
  }

  register() {
    if (this.registerUser.username === '' || this.registerUser.password === '') return;

    this.accountService.register(this.registerUser).subscribe({
      next: (response) => {
        const resp = response as User;
        console.log(resp.userName);
        console.log(resp.token);
        console.log(resp);

        this.getUsers();
        this.cancel();
      },
      error: (error) => {
        console.log(error);
      }
    });
  }

  async cancel() {
    this.cancelRegister.emit(false);
  }

  checkUserName() {
    if (this.registerUser.username != '') {
      this.accountService.checkUserName(this.registerUser.username).subscribe(response => {
        console.log(response);
        if (response) this.usernameExists = true;
        else this.usernameExists = false;
      });
    }
    else
      this.usernameExists = undefined;
  }

  getUsers() {
    this.accountService.getUsers().subscribe({
        // The subscribe takes different callback functions to help it understand what to do
        // Next tells the get request what to do next with the data
        next: (users: any) => this.existingUsers = users,
        // Error tells the get request what to do when an error occurs
        error: (error: any) => {console.log(error)},
        // Complete tells the get request what to do when the request has completed
        complete: () => console.log("Request at endpoint /api/users has completed")
      })
  }
}
