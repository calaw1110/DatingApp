import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { take } from 'rxjs';
import { Member } from '../../_models/member';
import { Message } from '../../_models/message';
import { User } from '../../_models/user';
import { AccountService } from '../../_services/account.service';
import { PresenceService } from '../../_services/presence.service';
import { MessageService } from './../../_services/message.service';

@Component({
    selector: 'app-member-detail',
    templateUrl: './member-detail.component.html',
    styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit, OnDestroy {

    @ViewChild('memberTabs', { static: true }) memberTabs?: TabsetComponent;
    activeTab?: TabDirective;
    member!: Member;
    galleryOptions: NgxGalleryOptions[] = [];
    galleryImages: NgxGalleryImage[] = [];
    messages: Message[] = [];
    user?: User;
    constructor(private accountService: AccountService, private route: ActivatedRoute,
        private messageService: MessageService, public presenceService: PresenceService,
        private router: Router) {
        this.accountService.currentUser$.pipe(take(1))
            .subscribe({
                next: user => {
                    if (user) this.user = user
                }
            });
        this.router.routeReuseStrategy.shouldReuseRoute = () => false;
    }


    ngOnInit(): void {
        this.route.data.subscribe({
            next: data => {
                console.log("this.route.data", data);
                this.member = data['member']
            }
        })
        this.route.queryParams.subscribe({
            next: params => {
                console.log('this.route.queryParams', params);
                params['tab'] && this.selectTab(params['tab']);
            }
        })

        this.galleryOptions = [
            {
                width: '500px',
                height: '500px',
                imagePercent: 100,
                thumbnailsColumns: 4,
                imageAnimation: NgxGalleryAnimation.Slide,
                preview: false
            }
        ];
        this.galleryImages = this.getImages();
    }

    ngOnDestroy(): void {
        this.messageService.stopHubConnection();
    }

    /**
     * 取得會員圖片
     * @return {any[]} 會員圖片資訊陣列
     */
    getImages() {
        if (!this.member) return [];
        const imageUrls = [];
        for (const photo of this.member.photos) {
            // ngx-gallery照片連結設定
            imageUrls.push({
                small: photo.url,
                medium: photo.url,
                big: photo.url
            })
            console.log("imageUrls", imageUrls);
        }
        return imageUrls;
    }

    /**
     * 切換活頁籤tab
     * @param {string} heading
     */
    selectTab(heading: string) {
        console.log('selectTab');
        if (this.memberTabs)
            this.memberTabs.tabs.find(x => x.heading === heading)!.active = true;
    }

    /**
     * 取得登入者對特定會員的對話資訊
     */
    loadMessages() {
        console.log('loadMessages');
        if (this.member) {
            this.messageService.getMessageThread(this.member.userName).subscribe({
                next: messages => this.messages = messages
            })
        }
    }
    /**
     * 活頁籤監聽事件-切換到Messages觸發loadMessages()
     * @param {TabDirective} data
     */
    onTabActivated(data: TabDirective) {
        console.log('onTabActivated');
        this.activeTab = data;
        if (this.activeTab.heading === 'Messages' && this.user) {
            this.messageService.createHubConnection(this.user, this.member.userName);
        } else {
            this.messageService.stopHubConnection();
        }
    }
}
