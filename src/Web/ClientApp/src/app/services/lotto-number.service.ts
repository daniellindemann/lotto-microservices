import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { catchError, Observable, of, tap } from 'rxjs';
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
    debugger;
    const url = this.appConfigService.lottoService.url + '/api/lottonumber';
    return this.httpClient.get<LottoField>(url)
      .pipe(
        tap(_ => this.log('fetched lotto field')),
        catchError(this.handleError<LottoField>('getLottoNumber', {} as LottoField))
      );
  }

  public getHistory() {
    // const url = this.appConfigService.lottoService.url + 'api/lottonumber/history';
    // return this.httpClient.get<LottoField>(url);
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      debugger;
      console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      this.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }

  private log(message: string) {
    console.log(message);
  }
}
