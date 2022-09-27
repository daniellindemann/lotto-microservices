import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { tap } from 'rxjs';
import { LottoField } from '../models/lottoField';
import { AppConfigService } from './app-config.service';

@Injectable({
  providedIn: 'root'
})
export class LottoNumberService {

  constructor(private appConfigService: AppConfigService,
    private httpClient: HttpClient) {
  }

  public getLottoNumber() {
    const url = this.appConfigService.appConfig.lottoService.url + 'api/lottonumber';
    return this.httpClient.get<LottoField>(url);
  }

  public getHistory() {
    // const url = this.appConfigService.appConfig.lottoService.url + 'api/lottonumber/history';
    // return this.httpClient.get<LottoField>(url);
  }
}
