import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AccountService } from '../service/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();

  model: any = {}
  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }

  register() {
    console.log(this.model);
    this.accountService.register(this.model).subscribe({
      next: response => {
        this.cancel();
      },
      error(err) {
        console.error(err);
      },
    })
  }
  cancel() {
    console.log("cancel");
    this.cancelRegister.emit(false);
  }
}
