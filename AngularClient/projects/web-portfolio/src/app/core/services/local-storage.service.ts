import { Inject, Injectable } from '@angular/core';
import * as CryptoJS from 'crypto-js';

@Injectable({
  providedIn: 'root',
})
export class LocalStorageService {
  key: string;
  constructor(@Inject('CRYPTO-KEY') CRYPTO_KEY: string) {
    this.key = CRYPTO_KEY;
  }

  public saveData(key: string, value: any) {
    const data = JSON.stringify(value); 
    const encrypted = this.encrypt(data);
    localStorage.setItem(key, encrypted);
  }

  public getData(key: string) {
    let data = localStorage.getItem(key);
    if(!!!data) return null;
    const descripted = this.decrypt(data);
    if(!!!descripted) return null;
    return JSON.parse(descripted)
  }
  public removeData(key: string) {
    localStorage.removeItem(key);
  }

  public clearData() {
    localStorage.clear();
  }

  private encrypt(txt: string): string {
    return CryptoJS.AES.encrypt(txt, this.key).toString();
  }

  private decrypt(txtToDecrypt: string) {
    return CryptoJS.AES.decrypt(txtToDecrypt, this.key).toString(CryptoJS.enc.Utf8);
  }
}