import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import {AccountService} from "./account.service";

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(private accountService: AccountService, private router: Router) { }

  canActivate(): boolean {
    if (this.accountService.isAuthenticated) {
      return true;
    } else {
      this.router.navigate(['login']);
      return false;
    }
  }
}
