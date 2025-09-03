import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

import { Observable } from "rxjs";

import { Section } from "../interfaces/section";

@Injectable({
    providedIn: 'root'
})
export class SectionService {
    private apiUrl = 'http://localhost:5271/api/sections';

    constructor(private http: HttpClient) {}

    getSections(): Observable<Section[]> {
        return this.http.get<Section[]>(this.apiUrl);
    }
}