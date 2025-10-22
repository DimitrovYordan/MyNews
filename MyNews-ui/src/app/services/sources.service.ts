import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

import { Observable } from "rxjs";

import { environment } from "../../environments/environment";
import { Source } from "../interfaces/source";

@Injectable({
    providedIn: 'root'
})
export class SourcesService {
    constructor(private http: HttpClient) {}

    getSources(): Observable<Source[]> {
        return this.http.get<Source[]>(`${environment.apiUrl}/api/sources`);
    }
}