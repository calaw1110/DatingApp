import { Component, OnInit } from '@angular/core';
import { Pagination } from '../_models/Pagination';
import { Message } from '../_models/message';
import { MessageService } from '../_services/message.service';

@Component({
    selector: 'app-messages',
    templateUrl: './messages.component.html',
    styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
    messages: Message[] | undefined;// 訊息列表
    pagination?: Pagination; // 分頁資訊
    container = 'Unread'; // 訊息存放容器 (預設為未讀)
    pageNumber = 1; // 目前頁碼
    pageSize = 5; // 每頁訊息數量
    loading = false;


    constructor(private messageService: MessageService) { }

    ngOnInit(): void {
        this.loadMessages();
    }

    /**
     * 載入訊息資訊
     */
    loadMessages() {
        this.loading = true;
        this.messageService.getMessages(this.pageNumber, this.pageSize, this.container).subscribe({
            next: response => {
                console.log('loadMessages', response)
                this.messages = response.result;
                this.pagination = response.pagination;
                this.loading = false
            },
            error: error => {
                console.error(error);
            }
        })
    }

    /**
     * 刪除指定訊息
     * @param {number} id 訊息id
     */
    deleteMessage(id: number) {
        this.messageService.deleteMessage(id).subscribe({
            next: () => {
                // 從訊息列表中刪除指定 ID 的訊息
                this.messages?.splice(this.messages.findIndex(m => m.id === id), 1)
            }
        })
    }
    /**
     * 切換分頁
     * @param {*} event 切分頁觸發事件
     */
    pageChanged(event: any) {
        if (this.pageNumber !== event.page) {
            console.log('pageChanged');
            this.pageNumber = event.page;// 更新目前的頁數
            this.loadMessages();// 載入指定頁數的訊息列表
        }
    }

}
