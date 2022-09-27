import { BrowserModule } from '@angular/platform-browser';
import { APP_INITIALIZER, Inject, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { LottoNumberComponent } from './lotto-number/lotto-number.component';
import { Observable } from 'rxjs';
import { AppConfigService } from './services/app-config.service';

// function initializeAppFactory(httpClient: HttpClient, @Inject('BASE_URL') baseUrl: string): () => Observable<any> {
//   return () => httpClient.get<AppConfig>(baseUrl + 'api/weatherforecast')
//     .pipe(
//       tap
//     )
// }

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    LottoNumberComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'lotto-number', component: LottoNumberComponent }
    ])
  ],
  providers: [
    {
      provide: APP_INITIALIZER,
      multi: true,
      deps: [AppConfigService],
      useFactory: (appConfigService: AppConfigService) => () => appConfigService.loadAppConfig()
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
