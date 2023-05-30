import { Injectable } from '@angular/core';
import { AES, enc, mode, pad } from 'crypto-js/';
import { environment } from '../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class EncryptAndDecryptService {
    cryptKey = enc.Utf8.parse(environment.cryptKey);
    cryptIV = enc.Utf8.parse(environment.cryptIV);
    constructor() { }

    encryptAES(text: string) {
        const encrypted = AES.encrypt(text, this.cryptKey, {
            keySize: 128 / 8,
            iv: this.cryptIV,
            mode: mode.CBC,
            padding: pad.Pkcs7

        });
        return encrypted.toString();
    }
    decryptAES(text: string) {
        const decrypted = AES.decrypt(text, this.cryptKey, {
            keySize: 128 / 8,
            iv: this.cryptIV,
            mode: mode.CBC,
            padding: pad.Pkcs7
        });
        return decrypted.toString(enc.Utf8);
    }
}
