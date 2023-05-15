import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Message } from '../../_models/message';
import { MessageService } from '../../_services/message.service';

@Component({
    selector: 'app-member-messages',
    templateUrl: './member-messages.component.html',
    styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
    @ViewChild('messageForm') messageForm?: NgForm
    @Input() username?: string;
    @Input() messages: Message[] = [];

    messageContent!: string;
    loading: boolean = false;
    constructor(private messageService: MessageService) { }

    ngOnInit(): void {
        console.log('messages1', this.messages);
    }

    /**
     * 發送訊息
     */
    sendMessage(): void {
        if (!this.username) return; // 若沒有使用者名稱，則不進行發送訊息的動作
        this.loading = true; // 設定載入狀態為 true
        this.messageService.sendMessage(this.username, this.messageContent).subscribe({
            next: message => {
                this.messages.push(message); // 將新訊息加入訊息列表
                this.messageForm?.reset(); // 重設表單
                this.loading = false; // 設定載入狀態為 false
            }
        });
    }
}
