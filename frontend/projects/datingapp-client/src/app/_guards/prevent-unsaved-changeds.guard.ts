import { Injectable } from '@angular/core';
import {  CanDeactivate,  } from '@angular/router';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

@Injectable({
    providedIn: 'root'
})
export class PreventUnsavedChangedsGuard implements CanDeactivate<MemberEditComponent> {
    /**
     * 當使用者要離開這個 Guard 所防守的路由時，會觸發這個函式
     *
     * @param {MemberEditComponent} component - 該路由的 Component
     * @returns {(boolean)}
     * @memberof PreventUnsavedChangedsGuard
     */
    canDeactivate(component: MemberEditComponent,): boolean {
        if (component.editForm?.dirty) {
            return confirm('Are you sure you want to continue? Any unsaved changes will be lost');
        }
        return true;
    }

}
