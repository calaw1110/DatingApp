import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
    providedIn: 'root'
})
export class BusyService {
    busyReqyestCount = 0;

    constructor(private spinnerService: NgxSpinnerService) {
    }

    /**
     * 啟動畫面遮罩
     */
    busy() {
        this.busyReqyestCount++;
        this.spinnerService.show(undefined, {
            type: 'line-scale-pulse-out-rapid',
            bdColor: 'rgba(255,255,255,0.9)',
            color: '#333'
        })
    }
    /**
     * 關閉畫面遮罩
     */
    idle() {
        this.busyReqyestCount--;
        if (this.busyReqyestCount <= 0) {
            this.busyReqyestCount = 0;
            this.spinnerService.hide();
        }
    }
}
