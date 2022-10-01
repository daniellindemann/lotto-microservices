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

  lottoField?: LottoField;
  lottoFieldHistory: LottoField[] = [];

  constructor(private lottoNumberService: LottoNumberService) {
  }

  ngOnInit(): void {
    this.loadNumbers();
    this.loadHistory();
  }

  reload() {
    // var lottoServiceUrl = this.appConfigService.appConfig.LottoService.url;
    if (this.lottoFieldHistory != null) {
      this.lottoFieldHistory.unshift(this.lottoField || {} as LottoField);
    }
    this.loadNumbers();
  }

  private loadHistory() {
    this.lottoNumberService.getHistory()
      .subscribe(lottoFieldHistory => {
        this.lottoFieldHistory = lottoFieldHistory;
      });
  }

  private loadNumbers() {
    this.lottoNumberService.getLottoNumber()
      .subscribe(lottoField => {
        this.lottoField = lottoField;
      });
  }
}
