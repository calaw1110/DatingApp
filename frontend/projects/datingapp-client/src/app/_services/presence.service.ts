import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject, take } from 'rxjs';
import { environment } from '../../environments/environment';
import { User } from '../_models/user';

@Injectable({
    providedIn: 'root'
})
export class PresenceService {
    hubUrl = environment.hubUrl;
    private hubConnection?: HubConnection;
    private onlineUsersSource = new BehaviorSubject<string[]>([]);
    onlineUsers$ = this.onlineUsersSource.asObservable();
    constructor(private toastr: ToastrService, private router: Router) { }

    createHubConnection(user: User) {
        console.log("createHubConnection");
        this.hubConnection = new HubConnectionBuilder()
            .withUrl(this.hubUrl + 'presence', {
                accessTokenFactory: () => user.token
            })
            .withAutomaticReconnect() // 斷線自動重連
            .build();
        this.hubConnection.start().catch(error => {
            console.error(error)
        });
        this.hubConnection.on('UserIsOnline', username => {
            console.log("UserIsOnline");
            this.onlineUsers$.pipe(take(1)).subscribe({
                next: usernames => {
                    this.onlineUsersSource.next([...usernames, username]);
                }
            })
            this.toastr.info(username + ' has connected');
        });

        this.hubConnection.on('UserIsOffline', username => {
            console.log("UserIsOffline");
            this.onlineUsers$.pipe(take(1)).subscribe({
                next: usernames => {
                    this.onlineUsersSource.next(usernames.filter(x => x !== username));
                }
            })
            this.toastr.warning(username + ' has disconnected');
        });

        this.hubConnection.on("GetOnlineUsers", usernames => {
            this.onlineUsersSource.next(usernames);
        });

        this.hubConnection.on('NewMessageReceived',
            ({ username, knownAs }) => {
                this.toastr.info(knownAs + ' has sent you a new message!', '', {
                    positionClass: 'toast-top-right'
                })
                    .onTap.pipe(take(1)).subscribe({
                        next: () => this.router.navigateByUrl('/members/' + username + '?tab=Messages')
                    })
            })
    }
    stopHubConnection() {
        console.log("stopHubConnection");
        this.hubConnection?.stop()
            .catch(error => console.error(error))
    }
}
