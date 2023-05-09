import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Member } from '../_models/member';
import { map, of } from 'rxjs';
import { Photo } from '../_models/photo';
import { PaginatedResult } from '../_models/Pagination';

@Injectable({
    providedIn: 'root'
})
export class MembersService {
    baseUrl = environment.apiUrl;
    members: Member[] = [];
    paginatedResult: PaginatedResult<Member[]> = new PaginatedResult<Member[]>;

    constructor(private http: HttpClient) { }

    getMembers(page?: number, itemsPerPage?: number) {
        let params = new HttpParams();
        // 建立查詢分頁資訊參數
        if (page && itemsPerPage) {
            params = params.append('pageNumber', page);
            params = params.append('pageSize', itemsPerPage);
        }

        // if (this.members.length > 0) return of(this.members);
        return this.http.get<Member[]>(this.baseUrl + 'users', { observe: 'response', params }).pipe(
            map(response => {
                if (response.body) {
                    // 接body資料，查詢結果
                    this.paginatedResult.result = response.body
                }
                const pagination = response.headers.get('Pagination');
                if (pagination) {
                    // 接header資料，分頁資訊
                    this.paginatedResult.pagination = JSON.parse(pagination)
                }
                return this.paginatedResult;
            })
        )
    }

    getMember(username: string) {
        const member = this.members.find(x => x.userName === username);
        if (member) return of(member);
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
}
