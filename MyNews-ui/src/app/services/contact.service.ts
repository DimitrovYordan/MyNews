import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

import { Observable } from "rxjs";

import { ContactMessage } from "../interfaces/contact-message";

@Injectable({
    providedIn: 'root'
})
export class ContactService {
    private apiUrl = 'http://localhost:5271/api/contact';

    constructor(private http: HttpClient) { }

    sendMessage(msg: ContactMessage): Observable<any> {
        return this.http.post(this.apiUrl, msg);
    }
}