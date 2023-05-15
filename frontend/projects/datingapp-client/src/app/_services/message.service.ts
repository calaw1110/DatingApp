import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Message } from '../_models/message';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
    providedIn: 'root'
})
export class MessageService {
    /** API網址 */
    baseUrl = environment.apiUrl;

    /** 建構式 */
    constructor(private http: HttpClient) { }

    /**
     * 取得對話訊息
     * @param {number} pageNumber 頁碼
     * @param {number} pageSize 顯示比數
     * @param {string} container 信件容器
     * @return {*} 含分頁訊息的Response
     */
    getMessages(pageNumber: number, pageSize: number, container: string) {
        let params = getPaginationHeaders(pageNumber, pageSize);
        params = params.append('Container', container);
        return getPaginatedResult<Message[]>(this.baseUrl + 'messages', params, this.http);
    }

    /**
     * 取得指定用戶名相關的訊息Thread
     * @param {string} username - 用戶名
     * @returns {Observable<Message[]>} 特定用戶Thread的可觀察物件
     */
    getMessageThread(username: string) {
        return this.http.get<Message[]>(this.baseUrl + 'messages/thread/' + username);
    }

    /**
     * 發送訊息給指定的用戶
     * @param {string} username - 收件人的用戶名
     * @param {string} content - 訊息內容
     * @returns {Observable<Message>} 發送的訊息的可觀察物件
     */
    sendMessage(username: string, content: string) {
        return this.http.post<Message>(this.baseUrl + 'messages', { recipientUsername: username, content })
    }

    /**
     * 刪除指定ID的訊息
     * @param {number} id - 訊息ID
     * @returns {Observable<any>} 表示操作結果的可觀察物件
     */
    deleteMessage(id: number) {
        return this.http.delete(this.baseUrl + 'messages/' + id)
    }
}
