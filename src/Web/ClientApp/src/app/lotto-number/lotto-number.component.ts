import { Component } from '@angular/core';

@Component({
  selector: 'app-lotto-number-component',
  templateUrl: './lotto-number.component.html',
  styleUrls: ['lotto-number.component.css']
})
export class LottoNumberComponent {
  public currentCount = 0;

  public incrementCounter() {
    this.currentCount++;
  }
}
