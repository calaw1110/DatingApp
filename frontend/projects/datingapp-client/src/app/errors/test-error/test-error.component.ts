import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AccountService } from '../../service/account.service';

@Component({
    selector: 'app-test-error',
    templateUrl: './test-error.component.html',
    styleUrls: ['./test-error.component.css']
})
export class TestErrorComponent implements OnInit {

    baseUrl = 'https://localhost:5001/api/';

    validationErrors: string[] = []

    constructor(private http: HttpClient, private accountService: AccountService) { }

    ngOnInit(): void {
    }

    get404Error() {
        this.http.get(this.baseUrl + 'buggy/not-found').subscribe({
            next: (response) => console.log('next!', response),
            error: (err) => console.error('error!', err)
        })
    }
    get500Error() {
        this.http.get(this.baseUrl + 'buggy/server-error').subscribe({
            next: (response) => console.log('next!', response),
            error: (err) => console.error('error!', err)
        })
    }
    get401Error() {
        this.http.get(this.baseUrl + 'buggy/auth').subscribe({
            next: (response) => console.log('next!', response),
            error: (err) => console.error('error!', err)
        })
    }
    get400Error() {
        this.http.post(this.baseUrl + 'account/register', {}).subscribe({
            next: (response) => console.log('next!', response),
            error: (err) => {

                console.error('error!', err)

                this.validationErrors = err;
            }
        })
    }

}
