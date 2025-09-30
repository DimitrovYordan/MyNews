import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

import { Observable } from "rxjs";

import { Section } from "../interfaces/section";

@Injectable({
    providedIn: 'root'
})
export class SectionService {
    private apiUrl = 'http://localhost:5271/api/sections';

    allSections: Section[] = [];

    constructor(private http: HttpClient) {
        this.getSections().subscribe(sections => this.allSections = sections);
    }

    getSections(): Observable<Section[]> {
        return this.http.get<Section[]>(this.apiUrl);
    }
}