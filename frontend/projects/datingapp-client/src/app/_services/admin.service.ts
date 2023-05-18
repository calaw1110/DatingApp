import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { User } from '../_models/user';

@Injectable({
    providedIn: 'root'
})
export class AdminService {
    baseUrl = environment.apiUrl;

    constructor(private http: HttpClient) { }

    getRoles() {
        return this.http.get<any[]>(this.baseUrl + 'admin/all-roles');
    }

    getUsersWithRoles() {
        return this.http.get<User[]>(this.baseUrl + 'admin/users-with-roles');
    }

    updateUserRoles(username: string, roles: string[]) {
        console.log('updateUserRoles', username, roles);
        const memberRolesUpdateDto = {
            userName: username,
            roles: roles
        }
        return this.http.put<string[]>(this.baseUrl + 'admin/edit-roles/', memberRolesUpdateDto);
    }
}
