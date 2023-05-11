import { Component, OnInit } from '@angular/core';
import { MembersService } from '../_services/members.service';

@Component({
    selector: 'app-lists',
    templateUrl: './lists.component.html',
    styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
    LikeList: any = []
    predicate: string = 'liked'
    constructor(private membersService: MembersService) { }

    ngOnInit(): void {
        this.loadLikes()
    }
    loadLikes() {
        this.membersService.getLikes(this.predicate).subscribe({
            next: (response) => {
                console.log(response);
                this.LikeList = response;
                // this.list = response
            }
        })
    }
}
