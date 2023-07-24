import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, catchError } from 'rxjs';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    constructor(private router: Router, private toastr: ToastrService) { }
    intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
        return next.handle(request).pipe(
            catchError((err: HttpErrorResponse) => {
                if (err) {
                    switch (err.status) {
                        case 400:
                            if (err.error.errors) {
                                const modelStateErrors = [];
                                for (const key in err.error.errors) {
                                    if (err.error.errors[key]) {
                                        modelStateErrors.push(err.error.errors[key]);
                                    }
                                }
                                // Array.prototype.flat(n) ,n = 深度
                                // 將陣列中的子陣列 組成一個新陣列
                                // 預設不代n時 為深度1
                                // example:
                                // [1, 2, 3 ,[4, 5]].flat() => [1, 2, 3, 4, 5]
                                // [1, 2, [3, 4, [5, 6]]].flat() => [1, 2, 3, 4, [5, 6]]
                                // [1, 2, [3, 4, [5, 6]]].flat(2) => [1, 2, 3, 4, 5, 6]
                                throw modelStateErrors.flat();
                            } else {
                                this.toastr.error(err.error, err.status.toString());
                            }
                            break;
                        case 401:
                            this.toastr.error('Unauthorised', err.status.toString());
                            break;
                        case 404:
                            this.router.navigateByUrl('/not-found');
                            break;
                        case 500:
                            const navigationExtras: NavigationExtras = { state: { error: err.error } };
                            this.router.navigateByUrl('/server-error', navigationExtras);
                            break;
                        default:
                            this.toastr.error('Something unexpected went wrong');
                            console.error(err);
                            break;
                    }
                }
                throw err;
            })
        )
    }
}