import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { Member } from '../../_models/member';
import { User } from '../../_models/user';
import { AccountService } from '../../_services/account.service';
import { MembersService } from '../../_services/members.service';
import { take } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { NgForm } from '@angular/forms';

@Component({
    selector: 'app-member-edit',
    templateUrl: './member-edit.component.html',
    styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
    @ViewChild('editForm') editForm: NgForm | undefined;

    // 監聽瀏覽器事件
    @HostListener('window:beforeunload', ['$event']) inloadNotification($event: any) {
        if (this.editForm?.dirty) {
            $event.returnValue = true;
        }
    }
    member!: Member;
    user: User | null = null;
    constructor(private accountService: AccountService,
        private memberService: MembersService,
        private toastr: ToastrService
    ) {
        this.accountService.currentUser$.pipe(take(1)).subscribe({
            next: user => {
                this.user = user;
            }
        })
    }

    ngOnInit(): void {
        this.loadMember();
    }
    
    loadMember() {
        if (!this.user) return;
        this.memberService.getMember(this.user.username).subscribe({
            next: member => {
                this.member = member;
            }
        })
    }

    updateMember() {
        console.log('update member');
        this.memberService.updateMember(this.editForm?.value).subscribe({
            next: response => {
                this.toastr.success('Update Completed')
                this.editForm?.reset(this.member)
            }
        })
    }
}
