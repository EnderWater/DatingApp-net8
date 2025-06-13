import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { Login } from '../_interfaces/login';
import { createDefaultLogin } from '../_interfaces/factory';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { TitleCasePipe } from '@angular/common';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [FormsModule, BsDropdownModule, RouterLink, RouterLinkActive, TitleCasePipe],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  public accountService = inject(AccountService);
  public model: Login = createDefaultLogin();
  private router = inject(Router);
  private toastr = inject(ToastrService);

  ngOnInit() {
    const currentUser = this.accountService.currentUser();
    this.model.username = currentUser ? currentUser.userName : '';
  }

  async login() {
    (await this.accountService.login(this.model)).subscribe({
      next: () => {
        const currentUser = this.accountService.currentUser();
        this.model.username = currentUser ? currentUser.userName : '';
        this.router.navigateByUrl('/members');
      },
      error: error => this.toastr.error(error.error)
    });
  }

  async logout() {
    this.accountService.logout();
    this.model.password = '';
    this.router.navigateByUrl('/');
  }
}
