import { Injectable } from '@angular/core';

import { ToastrService } from 'ngx-toastr';
import { Observable, map } from 'rxjs';
import { AccountService } from '../_services/account.service';

@Injectable({
    providedIn: 'root'
})
export class AdminGuard  {
    constructor(private accountService: AccountService, private toastr: ToastrService) {
    }

    canActivate(): Observable<boolean> {
        // 使用者進入路由時的權限檢查
        return this.accountService.currentUser$.pipe(
            map(user => {
                if (!user) return false; // 若使用者未登入，禁止進入
                if (user.roles.includes('Admin') || user.roles.includes('Moderator')) {
                    // 若使用者具有 "Admin" 或 "Moderator" 角色，允許進入
                    return true;
                } else {
                    // 若使用者不具有指定角色，顯示錯誤訊息並禁止進入
                    this.toastr.error("You cannot enter this area");
                    return false;
                }
            })
        )
    }
}
