import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve } from '@angular/router';
import { Observable } from 'rxjs';
import { Member } from '../_models/member';
import { MembersService } from '../_services/members.service';

@Injectable({
    providedIn: 'root'
})
export class MemberDetailedResolver implements Resolve<Member> {

    constructor(private memberService: MembersService) { }

    /**
     * 解析會員詳細資訊
     * @param {ActivatedRouteSnapshot} route - 路由快照
     * @returns {Observable<Member>} 會員詳細資訊的可觀察物件
     */
    resolve(route: ActivatedRouteSnapshot): Observable<Member> {
        // 先從路由快照取得key為username的value
        // 帶入MembersService 取得 member資訊
        // 回傳 Obsevable物件
        return this.memberService.getMember(route.paramMap.get('username')!)
    }
}
