import { Component, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { Member } from '../../_models/member';

@Component({
    selector: 'app-merber-list',
    templateUrl: './merber-list.component.html',
    styleUrls: ['./merber-list.component.css']
})
export class MerberListComponent implements OnInit {
    members: Member[] = [];
    constructor(private membersService: MembersService) { }

    ngOnInit(): void {
        this.loadMembers()
    }

    loadMembers() {
        this.membersService.getMembers().subscribe({
            next: members => this.members = members,
        })
    }

}
