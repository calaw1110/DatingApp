import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { environment } from '../../environments/environment';
import { User } from '../_models/user';

@Injectable({
    providedIn: 'root'
})
export class AccountService {
    private baseUrl = environment.apiUrl;
    private currentUserSource = new BehaviorSubject<User | null>(null);
    currentUser$ = this.currentUserSource.asObservable();

    constructor(private http: HttpClient) { }

    /**
     * 登入
     * @param {any} model - 登入表單資料
     * @returns {Observable<User>} 使用者的可觀察物件
     */
    login(model: any) {
        return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
            map((response: User) => {
                const user = response;
                if (user) {
                    this.setCurrentUser(user);
                }
            })
        );
    }

    /**
     * 註冊
     * @param {any} model - 註冊表單資料
     * @returns {Observable<User>} 使用者的可觀察物件
     */
    register(model: any) {
        return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
            map(user => {
                if (user) {
                    this.setCurrentUser(user);
                }
            })
        );
    }

    /**
     * 設定目前使用者
     * @param {User} user - 要設定的使用者
     */
    setCurrentUser(user: User): void {
        user.roles = [];
        const roles = this.getDecodedToken(user.token).role;
        Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
        // 將 JWT 儲存在 localstorage
        localStorage.setItem('user', JSON.stringify(user));
        this.currentUserSource.next(user);
    }

    /**
     * 登出使用者
     */
    logout(): void {
        // 將 localstorage 的 JWT 移除
        localStorage.removeItem('user');
        this.currentUserSource.next(null);
    }

    getDecodedToken(token: string) {
        return JSON.parse(window.atob(token.split('.')[1]))
    }
}
