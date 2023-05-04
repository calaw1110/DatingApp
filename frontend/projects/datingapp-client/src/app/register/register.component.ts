import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
    @Output() cancelRegister = new EventEmitter();

    model: any = {}
    constructor(private accountService: AccountService, private toastr: ToastrService) { }

    ngOnInit(): void {
    }

    register() {
        console.log(this.model);
        this.accountService.register(this.model).subscribe({
            next: response => {
                this.cancel();
            }
        })
    }
    cancel() {
        console.log("cancel");
        this.cancelRegister.emit(false);
    }
}
