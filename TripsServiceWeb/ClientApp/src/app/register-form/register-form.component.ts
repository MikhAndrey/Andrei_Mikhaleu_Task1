import {Component} from '@angular/core';
import {SignupValidationErrors, UserSignupDTO} from "../../models/signup";
import {AccountService} from "../../services/account.service";
import {RedirectService} from "../../services/redirect.service";

@Component({
  selector: 'app-register-form',
  templateUrl: './register-form.component.html'
})
export class RegisterFormComponent {
  user: UserSignupDTO = new UserSignupDTO();
  validationErrors: SignupValidationErrors = {};

  constructor(private accountService: AccountService, private redirectService: RedirectService) {
  }

  register() {
    this.accountService.signup(this.user).subscribe({
      next: () => {
        this.redirectService.redirectToAddress("");
      },
      error: (error) => {
        this.validationErrors = error.error.errors || error.error;
      }
    });
  }
}

