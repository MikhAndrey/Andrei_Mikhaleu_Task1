import {Inject, Injectable} from "@angular/core";

@Injectable({ providedIn: 'root' })
export class RedirectService {

  constructor(@Inject('BASE_URL') private baseUrl: string) {
  }

  redirectToAddress(address: string): void{
    window.location.href = this.baseUrl + address;
  }
}
