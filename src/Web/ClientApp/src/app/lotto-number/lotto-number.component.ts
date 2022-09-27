import { Component } from '@angular/core';
import { AppConfigService } from '../services/app-config.service';

@Component({
  selector: 'app-lotto-number-component',
  templateUrl: './lotto-number.component.html',
  styleUrls: ['lotto-number.component.css']
})
export class LottoNumberComponent {

  constructor() {
  }

  reload() {
    debugger;
    // var lottoServiceUrl = this.appConfigService.appConfig.LottoService.url;

  }
}
