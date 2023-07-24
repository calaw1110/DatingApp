import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, take } from 'rxjs';
import { AccountService } from '../_services/account.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

    constructor(private accountService: AccountService) { }
    intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
        // 攔截 HTTP 請求並注入 JWT 標頭
        this.accountService.currentUser$.pipe(take(1)).subscribe({
            next: user => {
                if (user) {
                    // 若使用者已登入，將 JWT 標頭注入請求中
                    request = request.clone({
                        setHeaders: {
                            Authorization: `Bearer ${user.token}`
                        }
                    })
                }
            }
        })
        return next.handle(request);
    }
}
