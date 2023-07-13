import {Component} from '@angular/core';
import {LoginValidationErrors, UserLoginDTO} from "../../models/login";
import {AccountService} from "../../services/account.service";
import {RedirectService} from "../../services/redirect.service";

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html'
})
export class LoginFormComponent {
  user: UserLoginDTO = new UserLoginDTO();
  validationErrors: LoginValidationErrors = {};

  constructor(private accountService: AccountService, private redirectService: RedirectService) {
  }

  login() {
    this.accountService.login(this.user).subscribe({
      next: () => {
        this.redirectService.redirectToAddress("");
      },
      error: (error) => {
        this.validationErrors = error.error.errors || error.error;
      }
    });
  }
}

