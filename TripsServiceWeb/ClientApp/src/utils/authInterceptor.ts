import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
} from '@angular/common/http';
import { Router } from '@angular/router';
import {catchError, NEVER, Observable, throwError} from "rxjs";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private router: Router) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError(error => {
        if (!!error.status) {
          switch (error.status) {
            case 401:
              this.router.navigate(['login']);
              return NEVER;
            case 403:
              alert("You have no access to this page");
              this.router.navigate(['']);
              return NEVER;
          }
        }
        return throwError(error);
      })
    );
  }
}
