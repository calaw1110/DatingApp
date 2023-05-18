import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, take } from 'rxjs';
import { environment } from '../../environments/environment';
import { Group } from '../_models/Group';
import { Message } from '../_models/message';
import { User } from '../_models/user';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
    providedIn: 'root'
})
export class MessageService {
    /** API網址 */
    baseUrl = environment.apiUrl;
    hubUrl = environment.hubUrl;
    private hubConnection?: HubConnection;
    private messageThreadSource = new BehaviorSubject<Message[]>([]);
    messageThread$ = this.messageThreadSource.asObservable();
    /** 建構式 */
    constructor(private http: HttpClient) { }

    createHubConnection(user: User, otherUsername: string) {
        this.hubConnection = new HubConnectionBuilder()
            .withUrl(this.hubUrl + 'message?user=' + otherUsername, {
                accessTokenFactory: () => user.token
            })
            .withAutomaticReconnect()
            .build()

        this.hubConnection.start()
            .catch(
                error => console.error(error)
            );

        this.hubConnection.on('ReceiveMessageThread', message => {
            this.messageThreadSource.next(message);
        });

        this.hubConnection.on('UpdatedGroup', (group: Group) => {
            if (group.connections.some(x => x.username === otherUsername)) {
                this.messageThread$.pipe(take(1)).subscribe({
                    next: messages => {
                        messages.forEach(message => {
                            if (!message.dateRead) {
                                message.dateRead = new Date(Date.now())
                            }
                        });
                        this.messageThreadSource.next([...messages]);
                    }
                });
            }
        })


        this.hubConnection.on('NewMessage', message => {
            this.messageThread$.pipe(take(1)).subscribe({
                next: messages => {
                    this.messageThreadSource.next([...messages, message])
                    console.log("NewMessage", this.messageThreadSource);
                }
            })
        });
    }

    stopHubConnection() {
        console.log("stopHubConnection");
        if (this.hubConnection) {
            this.hubConnection?.stop()
                .catch(error => console.error(error))
        }
    }

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
    * 發送訊息給指定的使用者。
    * @param username 目標使用者名稱。
    * @param content 訊息內容。
    * @returns 一個 Promise，表示發送訊息的非同步操作結果。
    */
    async sendMessage(username: string, content: string) {
        // 使用MessageHub連線執行 'SendMessage' 方法
        // 傳遞一個物件作為參數，包含目標使用者名稱和訊息內容
        return this.hubConnection?.invoke('SendMessage', { recipientUsername: username, content })
            .catch(error => console.error('SendMessage', error))
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
