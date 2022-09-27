import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { tap } from 'rxjs';
import { AppConfig } from '../models/appConfig';

@Injectable({
  providedIn: 'root'
})
export class AppConfigService {

  appConfig: AppConfig = {} as AppConfig;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  loadAppConfig() {
    debugger;
    return this.http.get<AppConfig>(this.baseUrl + 'api/appconfig/get')
      .pipe(
        tap(value => this.appConfig = value)
      );
  }
}
