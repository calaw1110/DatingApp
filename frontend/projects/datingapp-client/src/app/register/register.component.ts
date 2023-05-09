import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Route, Router } from '@angular/router';
@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
    @Output() cancelRegister = new EventEmitter();
    registerForm: FormGroup = new FormGroup({});
    maxDate: Date = new Date();
    validationErrors: string[] = [];
    constructor(private accountService: AccountService,
        private toastr: ToastrService,
        private fb: FormBuilder,
        private router: Router) { }

    ngOnInit(): void {
        this.initializeForm();
        this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
    }


    initializeForm() {
        this.registerForm = this.fb.group({
            gender: ['male', Validators.required],
            username: ['', Validators.required],
            knownAs: ['', Validators.required],
            dateOfBirth: ['', Validators.required],
            city: ['', Validators.required],
            country: ['', Validators.required],
            password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
            confirmPassword: ['', [Validators.required, this.matchValues('password')]],
        });
        // 監聽password變化
        this.registerForm.controls['password'].valueChanges.subscribe({
            next: () => {
                this.registerForm.controls['confirmPassword'].updateValueAndValidity();
            }
        })
    }
    /**
     * 自定義欄位驗證
     * @param matchTo 目標欄位
     * @return 
     */
    matchValues(matchTo: string): ValidatorFn {
        return (control: AbstractControl) => {
            // 驗證 confirmPassword 是否和 password 一樣
            // 符合：回傳null
            // 不符合：回傳物件，自訂錯誤名稱
            return control.value === control.parent?.get(matchTo)?.value ? null : { noMatching: true }
        }
    }

    register() {

        // 調整日期格式
        const dob = this.getDateOnly(this.registerForm.controls['dateOfBirth'].value);

        // 使用 ...複製陣列，並轉換dateOfBirth的值
        const values = { ...this.registerForm.value, dateOfBirth: dob };
        console.log(values);

        // submit value
        this.accountService.register(values).subscribe({
            next: () => {
                this.router.navigateByUrl('/members')
            },
            error: error => {
                console.error(error);
                this.validationErrors = error
            }
        })
    }
    cancel() {
        console.log("cancel");
        this.cancelRegister.emit(false);
    }

    private getDateOnly(dob: string | undefined) {
        if (!dob) return;
        const theDob = new Date(dob);
        // theDob.toISOString() -> 2005-05-09T13:17:39.000Z
        return theDob.toISOString().substring(0, 10) // 格式化日期
    }
}
