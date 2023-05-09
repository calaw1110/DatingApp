import { UserParams } from './../../_models/userParam';
import { Component, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { Member } from '../../_models/member';
import { take } from 'rxjs';
import { Pagination } from '../../_models/Pagination';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';
import { AccountService } from '../../_services/account.service';
import { User } from '../../_models/user';

@Component({
    selector: 'app-member-list',
    templateUrl: './member-list.component.html',
    styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

    members: Member[] | undefined;
    pagination: Pagination | undefined;
    userParams!: UserParams;
    user!: User;
    genderList = [{ value: 'male', display: 'Males' }, { value: 'female', display: 'Females' }]

    constructor(private membersService: MembersService, private accountService: AccountService) {
        this.accountService.currentUser$.pipe(take(1)).subscribe({
            next: user => {
                if (user) {
                    this.user = user;
                    this.userParams = new UserParams(this.user);
                }
            }
        })
    }

    ngOnInit(): void {
        this.loadMembers();
    }

    loadMembers() {
        if (!this.userParams) return;
        this.membersService.getMembers(this.userParams).subscribe({
            next: response => {
                if (response.result && response.pagination) {
                    this.members = response.result;
                    this.pagination = response.pagination;
                }
            }
        })
    }

    resetFilters() {
        if (this.user) {
            this.userParams = new UserParams(this.user);
            this.loadMembers();
        }
    }

    pageChaged(event: PageChangedEvent) {
        console.log(event);
        if (this.userParams && this.userParams.pageNumber !== event.page) {
            this.userParams.pageNumber = event.page;
            this.loadMembers()
        }
    }
}
