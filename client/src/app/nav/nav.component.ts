import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { Login } from '../_interfaces/login';
import { createDefaultLogin } from '../_interfaces/factory';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, BsDropdownModule],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  public accountService = inject(AccountService);
  public model: Login = createDefaultLogin();

  ngOnInit() {
    const currentUser = this.accountService.currentUser();
    this.model.username = currentUser ? currentUser.userName : '';
  }

  async login() {
    (await this.accountService.login(this.model)).subscribe({
      next: () => {
        const currentUser = this.accountService.currentUser();
        this.model.username = currentUser ? currentUser.userName : '';
      },
      error: error => console.log(error)
    });
  }

  async logout() {
    this.accountService.logout();
  }
}
