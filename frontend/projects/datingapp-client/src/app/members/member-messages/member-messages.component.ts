import { ChangeDetectionStrategy, Component, Input, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Message } from '../../_models/message';
import { MessageService } from '../../_services/message.service';

@Component({
    changeDetection: ChangeDetectionStrategy.OnPush,
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
    constructor(public messageService: MessageService) { }

    ngOnInit(): void {
    }

    /**
     * 發送訊息
     */
    sendMessage(): void {
        if (!this.username) return; // 若沒有使用者名稱，則不進行發送訊息的動作
        this.loading = true; // 設定載入狀態為 true
        this.messageService.sendMessage(this.username, this.messageContent).then(() => {
            this.messageForm?.reset();
        }).finally(() => {
            this.loading = false;
        })
    }
}
