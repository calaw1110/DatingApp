import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';
import { ConfirmService } from './../_services/confirm.service';

@Injectable({
    providedIn: 'root'
})
export class PreventUnsavedChangedsGuard  {
    /**
     * 當使用者要離開這個 Guard 所防守的路由時，會觸發這個函式
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
