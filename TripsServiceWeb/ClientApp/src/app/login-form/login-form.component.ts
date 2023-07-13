import {Component} from '@angular/core';
import {LoginValidationErrors, UserLoginDTO} from "../../models/login";
import {AccountService} from "../../services/account.service";

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html'
})
export class LoginFormComponent {
  user: UserLoginDTO = {};
  validationErrors: LoginValidationErrors = {};

  constructor(private accountService: AccountService) {
  }

  login() {
    this.accountService.login(this.user).subscribe({
      next: () => {
        this.accountService.returnToHomePage();
      },
      error: (error) => {
        this.validationErrors = error.error.errors || error.error;
      }
    });
  }
}

