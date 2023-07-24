import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import {AccountService} from "./account.service";
import {firstValueFrom} from "rxjs";

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(private accountService: AccountService, private router: Router) { }

  async canActivate(): Promise<boolean> {
    const response = await firstValueFrom(this.accountService.isAuthenticated());
    if (response) {
      return true;
    } else {
      await this.router.navigate(['login']);
      return false;
    }
  }
}
