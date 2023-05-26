import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map, of, take } from 'rxjs';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { User } from '../_models/user';
import { Like } from './../_models/like';
import { UserParams } from './../_models/userParam';
import { AccountService } from './account.service';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
    providedIn: 'root'
})
export class MembersService {
    /** API網址 */
    baseUrl = environment.apiUrl;

    /** 會員資料 */
    members: Member[] = [];

    /** 會員資料的快取 */
    memberCache = new Map();

    /** 使用者資料 */
    user: User | undefined;

    /** 使用者查詢參數 */
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

    /**
    * 取得使用者參數
    * @returns {UserParams | undefined} 使用者參數
    */
    getUserParams(): UserParams | undefined {
        return this.userParams;
    }

    /**
    * 設定使用者參數
    * @param {UserParams} params - 使用者參數
    */
    setUserParams(params: UserParams) {
        this.userParams = params;
    }

    /**
    * 重置使用者參數
    * @returns {UserParams | undefined} 重置後的使用者參數
    */
    resetUserParams() {
        if (this.user) {
            this.userParams = new UserParams(this.user);
            return this.userParams;
        }
        return;
    }


    /**
    * 取得會員列表
    * @param {UserParams} userParams - 使用者參數
    * @returns {Observable<PaginatedResult<Member[]>>} 成員列表的可觀察物件
    */
    getMembers(userParams: UserParams) {

        // 嘗試取得會員快取資料 
        const response = this.memberCache.get(Object.values(userParams).join('-'));

        // 若有快取資料，將其轉換成 Observable 物件並返回
        if (response) return of(response);

        // 無會員快取資料執行以下

        // 建立會員查詢參數
        let params = getPaginationHeaders(userParams.pageNumber, userParams.pageSize);
        params = params.append('minAge', userParams.minAge);
        params = params.append('maxAge', userParams.maxAge);
        params = params.append('gender', userParams.gender);
        params = params.append('orderBy', userParams.orderBy);


        return getPaginatedResult<Member[]>(this.baseUrl + 'users', params, this.http).pipe(
            map(response => {
                this.memberCache.set(Object.values(userParams).join('-'), response);
                return response;
            })
        )
    }

    /**
     * 取得指定成員
     * @param {string} username - 使用者名稱
     * @returns {Observable<Member>} 成員的可觀察物件
     */
    getMember(username: string): Observable<Member> {

        const member = [...this.memberCache.values()] // 將會員快取的值轉換為陣列
            .reduce((arr, elem) => arr.concat(elem.result), []) // 將每個快取項目的 result 屬性組合成一個陣列
            .find((member: Member) => member.userName === username); // 在陣列中尋找指定使用者名稱的成員

        // 若有快取資料，將其轉換成 Observable 物件並返回 
        if (member) return of(member);

        // 若無快取資料，執行api
        return this.http.get<Member>(this.baseUrl + 'users/' + username)
    }

    /**
     * 更新成員資訊
     * @param {Member} member - 要更新的成員
     * @returns {Observable<void>} 可觀察物件
     */
    updateMember(member: Member): Observable<void> {
        return this.http.put(this.baseUrl + 'users', member).pipe(
            map(() => {
                // 取得會員在會員列表中index位置
                const index = this.members.indexOf(member);

                // 使用物件合併的方式，將原始會員資訊和更新的會員資訊合併，以更新會員列表中特定會員的資訊
                this.members[index] = { ...this.members[index], ...member };
            })
        )
    }

    /**
     * 設定指定照片為主要照片
     * @param {number} photoId - 要設定為主要照片的照片 ID
     * @returns {Observable<any>} 可觀察物件
     */
    setMainPhoto(photoId: number): Observable<any> {
        // 執行 HTTP PUT 請求，將指定照片設定為主要照片，並返回一個可觀察物件
        return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});
    }

    /**
     * 刪除指定照片
     * @param {number} photoId - 要刪除的照片 ID
     * @returns {Observable<any>} 可觀察物件
     */
    deletePhoto(photoId: number): Observable<any> {
        // 執行 HTTP DELETE 請求，刪除指定照片，並返回一個可觀察物件
        return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
    }
    /**
     * add likes
     * @param {string} username - 要喜歡的使用者的使用者名稱
     * @returns {Observable<any>} 可觀察物件
     */
    addLike(username: string) {
        return this.http.post(this.baseUrl + 'likes/' + username, {});
    }

    /**
     * get likes
     * @param {string} predicate - 過濾條件
     * @param {number} pageNumber - 目標頁碼
     * @param {number} pageSize - 顯示筆數
     * @returns {Observable<any>} 可觀察物件
     */
    getLikes(predicate: string, pageNumber: number, pageSize: number) {
        // like list 查詢參數
        const userLikeListParams = { 'predicate': predicate, 'pageNumber': pageNumber, 'pageSize': pageSize };

        let params = getPaginationHeaders(userLikeListParams.pageNumber, userLikeListParams.pageSize);
        params = params.append('predicate', userLikeListParams.predicate);

        return getPaginatedResult<Like[]>(this.baseUrl + 'likes', params, this.http).pipe(
            map(response => {
                return response;
            })
        );
    }
}
