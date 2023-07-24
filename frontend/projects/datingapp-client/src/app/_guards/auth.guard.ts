import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, map } from 'rxjs';
import { AccountService } from '../_services/account.service';

@Injectable({
    providedIn: 'root'
})
export class AuthGuard implements CanActivate {

    constructor(private accountService: AccountService, private toastr: ToastrService) {

    }

    canActivate(): Observable<boolean> {
        // 使用者進入路由時的身份驗證
        return this.accountService.currentUser$.pipe(
            map(user => {
                if (user) return true; // 若使用者已登入，允許進入
                else {
                    // 若使用者未登入，顯示錯誤訊息並禁止進入
                    this.toastr.error('You shall not pass');
                    return false;
                }
            })
        )
    }

}
