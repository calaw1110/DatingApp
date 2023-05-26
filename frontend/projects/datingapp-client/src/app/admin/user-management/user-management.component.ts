import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { User } from '../../_models/user';
import { AdminService } from '../../_services/admin.service';
import { RolesModalComponent } from '../../modals/roles-modal/roles-modal.component';

@Component({
    selector: 'app-user-management',
    templateUrl: './user-management.component.html',
    styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {
    users!: User[];
    availableRoles: any[] = [];
    bsModalRef: BsModalRef<RolesModalComponent> = new BsModalRef<RolesModalComponent>();
    constructor(private adminService: AdminService, private modalService: BsModalService, ) { }

    ngOnInit(): void {
        this.getUsersWithRoles();
        this.getAllRoles();
    }

    getUsersWithRoles() {
        this.adminService.getUsersWithRoles().subscribe({
            next: users => {
                console.log('getUsersWithRoles', users);
                this.users = users;
            }
        })
    }
    getAllRoles() {
        this.adminService.getRoles().subscribe({
            next: roles => {
                if (roles) {
                    console.log('getAllRoles', roles);
                    this.availableRoles = roles
                }
            }
        })
    }
    openRolesModal(user: User) {
        const config = {
            class: 'modal-dialog-centered',
            initialState: {
                username: user.username,
                availableRoles: this.availableRoles,
                selectedRoles: [...user.roles]
            }
        };
        this.bsModalRef = this.modalService.show(RolesModalComponent, config);
        this.bsModalRef.onHidden?.subscribe({
            next: () => {
                const selectedRoles = this.bsModalRef.content?.selectedRoles;
                if (!this.arrayEqual(selectedRoles!, user.roles)) {
                    this.adminService.updateUserRoles(user.username, selectedRoles!).subscribe({
                        next: roles => user.roles = roles,
                    })
                }
            }
        });
    }
    private arrayEqual(arr1: any[], arr2: any[]) {
        return JSON.stringify(arr1.sort()) === JSON.stringify(arr2.sort());
    }
}
