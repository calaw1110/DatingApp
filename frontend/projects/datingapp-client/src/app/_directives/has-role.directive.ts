import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { take } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from './../_services/account.service';

@Directive({
    selector: '[appHasRole]' // *appHasRole ='["Admin","Moderatoer"]'
})
export class HasRoleDirective {
    @Input() appHasRole: string[] = [];
    user: User = {} as User;

    constructor(private viewContainerRef: ViewContainerRef, private templateRef: TemplateRef<any>,
        private accountService: AccountService) {
        this.accountService.currentUser$.pipe(take(1)).subscribe({
            next: user => {
                if (user) this.user = user
            }
        })
    }

    ngOnInit(): void {
        // 在指令初始化時進行檢查，檢查當前使用者是否具有指定角色
        if (this.user.roles.some(r => this.appHasRole.includes(r))) {
            // 若使用者具有指定角色，則將對應的模板加入視圖容器中
            this.viewContainerRef.createEmbeddedView(this.templateRef);
        } else {
            // 若使用者不具有指定角色，則清空視圖容器，不顯示相關內容
            this.viewContainerRef.clear();
        }
    }
}
