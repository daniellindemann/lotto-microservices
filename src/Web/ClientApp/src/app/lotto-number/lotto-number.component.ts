import { Component, OnInit } from '@angular/core';
import { LottoField } from '../models/lottoField';
import { AppConfigService } from '../services/app-config.service';
import { LottoNumberService } from '../services/lotto-number.service';

@Component({
  selector: 'app-lotto-number-component',
  templateUrl: './lotto-number.component.html',
  styleUrls: ['lotto-number.component.css']
})
export class LottoNumberComponent implements OnInit {
  lottoField: LottoField = {} as LottoField;

  constructor(private lottoNumberService: LottoNumberService) {
  }

  ngOnInit(): void {
    this.loadNumbers();
  }

  reload() {
    // var lottoServiceUrl = this.appConfigService.appConfig.LottoService.url;
    this.loadNumbers();
  }

  private loadNumbers() {
    this.lottoNumberService.getLottoNumber()
      .subscribe(lottoField => {
        debugger;
        this.lottoField = lottoField;
      });
  }
}
