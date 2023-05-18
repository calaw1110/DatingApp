import { Component, OnInit } from '@angular/core';
import { Pagination } from '../_models/Pagination';
import { MembersService } from '../_services/members.service';

@Component({
    selector: 'app-lists',
    templateUrl: './lists.component.html',
    styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
    LikeList: any = []
    predicate: string = 'liked';
    pageNumber = 1;
    pageSize = 5;
    pagination: Pagination | undefined;
    constructor(private membersService: MembersService) { }

    ngOnInit(): void {
        this.loadLikes()
    }
    loadLikes() {
        this.membersService.getLikes(this.predicate, this.pageNumber, this.pageSize).subscribe({
            next: (response) => {
                console.log(response);
                this.LikeList = response.result;
                this.pagination = response.pagination;
                // this.list = response
            }
        })
    }

    pageChanged(event: any) {
        if (this.pageNumber !== event.page) {
            this.pageNumber = event.page;
            this.loadLikes()
        }
    }
}
