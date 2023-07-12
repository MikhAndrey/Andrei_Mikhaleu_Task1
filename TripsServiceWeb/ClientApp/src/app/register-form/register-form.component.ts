import {Component, Inject} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {SignupValidationErrors, UserSignupDTO} from "../../models/signup";

@Component({
  selector: 'app-register-form',
  templateUrl: './register-form.component.html'
})
export class RegisterFormComponent {
  user: UserSignupDTO = {};
  validationErrors: SignupValidationErrors = {};

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  register() {
    this.http.post(this.baseUrl + 'api/account/register', this.user).subscribe(
      () => {
        window.location.href = this.baseUrl;
      },
      (error) => {
        this.validationErrors = error.error.errors || error.error;
      }
    );
  }
}

