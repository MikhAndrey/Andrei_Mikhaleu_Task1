import { Component } from '@angular/core';
import {AccountService, UserNameResponse} from "../../services/account.service";
import {RedirectService} from "../../services/redirect.service";

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  userName?: string;

  constructor(private accountService: AccountService, private redirectionService: RedirectService) {
  }

  ngOnInit(): void {
    this.accountService.getUserName().subscribe(
      (response: UserNameResponse) => {
        this.userName = response.userName
      }
    );
  }

  logout(): void {
    this.accountService.logout().subscribe(
      () => {
        this.userName = undefined;
        this.redirectionService.redirectToAddress("");
      }
    );
  }
}
