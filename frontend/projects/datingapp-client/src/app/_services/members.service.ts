import { UserParams } from './../_models/userParam';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Member } from '../_models/member';
import { map, of, take } from 'rxjs';
import { PaginatedResult } from '../_models/Pagination';
import { AccountService } from './account.service';
import { User } from '../_models/user';

@Injectable({
    providedIn: 'root'
})
export class MembersService {
    baseUrl = environment.apiUrl;
    members: Member[] = [];
    memberCache = new Map();
    user: User | undefined;
    userParams: UserParams | undefined;


    constructor(private http: HttpClient, private accountService: AccountService) {
        this.accountService.currentUser$.pipe(take(1)).subscribe({
            next: user => {
                if (user) {
                    this.user = user;
                    this.userParams = new UserParams(this.user);
                }
            }
        })
    }

    getUserParams() {
        return this.userParams;
    }

    setUserParams(params: UserParams) {
        this.userParams = params;
    }

    resetUserParams() {
        if (this.user) {
            this.userParams = new UserParams(this.user);
            return this.userParams;
        }
        return;
    }

    getMembers(userParams: UserParams) {
        const response = this.memberCache.get(Object.values(userParams).join('-'));
        console.log(this.memberCache);
        if (response) return of(response);


        // 建立查詢分頁資訊參數
        let params = this.getPaginationHeaders(userParams.pageNumber, userParams.pageSize);

        // 搜尋條件、參數
        params = params.append('minAge', userParams.minAge);
        params = params.append('maxAge', userParams.maxAge);
        params = params.append('gender', userParams.gender);
        params = params.append('orderBy', userParams.orderBy);

        // if (this.members.length > 0) return of(this.members);
        return this.getPaginatedResult<Member[]>(this.baseUrl + 'users', params).pipe(
            map(response => {
                console.log(response);
                this.memberCache.set(Object.values(userParams).join('-'), response);
                return response;
            })
        )
    }

    getMember(username: string) {
        const member = [...this.memberCache.values()]
            .reduce((arr, elem) => arr.concat(elem.result), [])
            .find((member: Member) => member.userName === username)
        console.log('getMember', member);
        return this.http.get<Member>(this.baseUrl + 'users/' + username)
    }

    updateMember(member: Member) {
        return this.http.put(this.baseUrl + 'users', member).pipe(
            map(() => {
                const index = this.members.indexOf(member);
                this.members[index] = { ...this.members[index], ...member };
            })
        )
    }

    setMainPhoto(photoId: number) {
        return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});
    }

    deletePhoto(photoId: number) {
        return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
    }

    private getPaginatedResult<T>(url: string, params: HttpParams) {
        const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>;
        return this.http.get<T>(url, { observe: 'response', params }).pipe(
            map(response => {
                if (response.body) {
                    // 接body資料，查詢結果
                    paginatedResult.result = response.body;
                }
                const pagination = response.headers.get('Pagination');
                if (pagination) {
                    // 接header資料，分頁資訊
                    paginatedResult.pagination = JSON.parse(pagination);
                }
                return paginatedResult;
            })
        );
    }

    private getPaginationHeaders(pageNumber: number, pageSize: number) {
        let params = new HttpParams();
        params = params.append('pageNumber', pageNumber);
        params = params.append('pageSize', pageSize);
        return params;
    }
}
