import { Component, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { Member } from '../../_models/member';
import { Observable } from 'rxjs';

@Component({
    selector: 'app-member-list',
    templateUrl: './member-list.component.html',
    styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
    members$: Observable<Member[]> | undefined;
    constructor(private membersService: MembersService) { }

    ngOnInit(): void {
        this.members$ = this.membersService.getMembers();
    }



}
