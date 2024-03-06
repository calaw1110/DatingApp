import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';
import { ConfirmService } from './../_services/confirm.service';

@Injectable({
    providedIn: 'root'
})
export class PreventUnsavedChangedsGuard  {
    /**
     * 當使用者要離開的頁面含有 form 表單 且表單有被異動過 就會觸發
     *
     * @param {MemberEditComponent} component - 該路由的 Component
     * @returns {(boolean)}
     * @memberof PreventUnsavedChangedsGuard
     */
    constructor(private confirmService: ConfirmService) { }
    canDeactivate(component: MemberEditComponent,): Observable<boolean> {
        if (component.editForm?.dirty) {
            return this.confirmService.confirm()
        }
        return of(true);
    }

}
