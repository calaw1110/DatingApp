import { Component, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { Member } from '../../_models/member';
import { Observable } from 'rxjs';
import { Pagination } from '../../_models/Pagination';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';

@Component({
    selector: 'app-member-list',
    templateUrl: './member-list.component.html',
    styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

    members: Member[] | undefined;
    pagination: Pagination | undefined;
    pageNumber = 1;
    pageSize = 5;
    constructor(private membersService: MembersService) { }

    ngOnInit(): void {
        this.loadMembers();
    }

    loadMembers() {
        this.membersService.getMembers(this.pageNumber, this.pageSize).subscribe({
            next: response => {
                if (response.result && response.pagination) {
                    this.members = response.result;
                    this.pagination = response.pagination;
                }
            }
        })
    }
    pageChaged(event: PageChangedEvent) {
        console.log(event);
         this.pageNumber = event.page;
         this.loadMembers()
    }
}
