import { Component, ElementRef, HostListener, Input, OnInit, QueryList, ViewChildren } from '@angular/core';
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
  @ViewChildren('countryItem') countryItems!: QueryList<ElementRef>;
  @ViewChildren('cityItem') cityItems!: QueryList<ElementRef>;

  countryActiveIndex = -1;
  cityActiveIndex = -1;
  countries: any[] = [];
  cities: string[] = [];
  filteredCountries: any[] = [];
  filteredCities: string[] = [];
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

  filterCountries() {
    const query = (this.formGroup.get('country')?.value || '').toLowerCase();

    this.filteredCountries = this.countries
      .filter(c => c.country.toLowerCase().includes(query));

    this.countryActiveIndex = this.filteredCountries.length ? 0 : -1;
  }

  selectCountry(country: string) {
    this.formGroup.get('country')?.setValue(country);

    const match = this.countries.find(x => x.country === country);
    this.cities = match ? match.cities : [];

    this.filteredCountries = [];
    this.filteredCities = [];
    this.countryActiveIndex = -1;
    this.cityActiveIndex = -1;

    this.formGroup.get('city')?.setValue('');
  }

  filterCities() {
    const query = (this.formGroup.get('city')?.value || '').toLowerCase();

    this.filteredCities = this.cities
      .filter(city => city.toLowerCase().includes(query));

    this.cityActiveIndex = this.filteredCities.length ? 0 : -1;
  }

  selectCity(city: string) {
    this.formGroup.get('city')?.setValue(city);
    this.filteredCities = [];
    this.cityActiveIndex = -1;
  }

  onCountryKeyDown(event: KeyboardEvent) {
    if (this.filteredCountries.length === 0) return;

    if (event.key === 'ArrowDown') {
      event.preventDefault();
      this.countryActiveIndex = (this.countryActiveIndex + 1) % this.filteredCountries.length;
      setTimeout(() => this.scrollToActive(this.countryItems, this.countryActiveIndex));
    }

    if (event.key === 'ArrowUp') {
      event.preventDefault();
      this.countryActiveIndex =
        (this.countryActiveIndex - 1 + this.filteredCountries.length) % this.filteredCountries.length;
      setTimeout(() => this.scrollToActive(this.countryItems, this.countryActiveIndex));
    }

    if (event.key === 'Enter' && this.countryActiveIndex >= 0) {
      event.preventDefault();
      const selected = this.filteredCountries[this.countryActiveIndex].country;
      this.selectCountry(selected);
    }

    if (event.key === 'Escape') {
      this.filteredCountries = [];
      this.countryActiveIndex = -1;
    }
  }

  onCityKeyDown(event: KeyboardEvent) {
    if (this.filteredCities.length === 0) return;

    if (event.key === 'ArrowDown') {
      event.preventDefault();
      this.cityActiveIndex = (this.cityActiveIndex + 1) % this.filteredCities.length;
      setTimeout(() => this.scrollToActive(this.cityItems, this.cityActiveIndex));
    }

    if (event.key === 'ArrowUp') {
      event.preventDefault();
      this.cityActiveIndex =
        (this.cityActiveIndex - 1 + this.filteredCities.length) % this.filteredCities.length;
      setTimeout(() => this.scrollToActive(this.cityItems, this.cityActiveIndex));
    }

    if (event.key === 'Enter' && this.cityActiveIndex >= 0) {
      event.preventDefault();
      const selected = this.filteredCities[this.cityActiveIndex];
      this.selectCity(selected);
    }

    if (event.key === 'Escape') {
      this.filteredCities = [];
      this.cityActiveIndex = -1;
    }
  }

  scrollToActive(items: QueryList<ElementRef>, index: number) {
    const elements = items.toArray();
    if (elements && elements[index]) {
      try {
        elements[index].nativeElement.scrollIntoView({
          behavior: 'smooth',
          block: 'nearest',
        });
      } catch { }
    }
  }

  @HostListener('document:click', ['$event'])
  onClickOutside(event: Event) {
    const target = event.target as HTMLElement;
    if (!target.closest('.autocomplete-wrapper')) {
      this.filteredCountries = [];
      this.filteredCities = [];
      this.countryActiveIndex = -1;
      this.cityActiveIndex = -1;
    }
  }
}