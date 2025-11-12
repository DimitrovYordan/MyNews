import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-location-selector',
  imports: [TranslateModule, ReactiveFormsModule],
  templateUrl: './location-selector.html',
  styleUrls: ['./location-selector.scss'],
})
export class LocationSelectorComponent implements OnInit {
  @Input() formGroup!: FormGroup;

  countries: any[] = [];
  cities: string[] = [];
  loading = true;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    const cache = localStorage.getItem('countriesData');
    if (cache) {
      this.countries = JSON.parse(cache);
      this.loading = false;
      this.populateCitiesIfCountrySelected();
    } else {
      this.http.get<any>('https://countriesnow.space/api/v0.1/countries')
        .subscribe({
          next: res => {
            this.countries = res.data;
            localStorage.setItem('countriesData', JSON.stringify(this.countries));
            this.loading = false;
            this.populateCitiesIfCountrySelected();
          },
          error: () => {
            this.loading = false;
          }
        });
    }
  }

  private populateCitiesIfCountrySelected() {
    const current = this.formGroup.get('country')?.value;
    if (current) {
      const found = this.countries.find(c => c.country === current);
      this.cities = found ? found.cities : [];
    }
  }

  onCountryChange() {
    const selectedCountry = this.formGroup.get('country')?.value;
    const match = this.countries.find(c => c.country === selectedCountry);
    this.cities = match ? match.cities : [];
    this.formGroup.get('city')?.setValue('');
  }
}