import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpResponse,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { switchMap, tap } from 'rxjs/operators';
import { AuthService } from '../auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthInterceptorService implements HttpInterceptor {
  constructor(private authService: AuthService) {}
  public intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return this.authService.getAccessToken().pipe(
      switchMap((token) => {
        if (token) {
          req = req.clone({
            headers: req.headers.set('Authorization', `Bearer ${token}`),
          });
        }

        return next.handle(req).pipe(
          tap(
            (event: any) => {
              if (event instanceof HttpResponse) {
                console.log('Server response');
              }
            },
            (err: any) => {
              if (err instanceof HttpErrorResponse) {
                if (err.status == 401) {
                  console.log('Unauthorized');
                }
              }
            }
          )
        );
      })
    );
  }
}
