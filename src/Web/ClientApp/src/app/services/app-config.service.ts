import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { tap } from 'rxjs';
import { AppConfig } from '../models/appConfig';
import { LottoServiceConfig } from '../models/lottoServiceConfig';

@Injectable({
  providedIn: 'root'
})
export class AppConfigService {

  private appConfig: AppConfig = {} as AppConfig;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  loadAppConfig() {
    return this.http.get<AppConfig>(this.baseUrl + 'api/appconfig/get')
      .pipe(
        tap(value => this.appConfig = value)
      );
  }

  get lottoService(): LottoServiceConfig {
    if (!this.appConfig.lottoService) {
      throw Error('Config not loaded');
    }

    return this.appConfig.lottoService;
  }
}
