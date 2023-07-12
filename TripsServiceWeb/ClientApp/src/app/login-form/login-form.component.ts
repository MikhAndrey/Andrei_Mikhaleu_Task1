import {Component, Inject} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {LoginValidationErrors, UserLoginDTO} from "../../models/login";

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html'
})
export class LoginFormComponent {
  user: UserLoginDTO = {};
  validationErrors: LoginValidationErrors = {};

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  login() {
    this.http.post(this.baseUrl + 'api/account/login', this.user).subscribe(
      (response) => {
        window.location.href = this.baseUrl;
      },
      (error) => {
        this.validationErrors = error.error.errors || error.error;
      }
    );
  }
}

