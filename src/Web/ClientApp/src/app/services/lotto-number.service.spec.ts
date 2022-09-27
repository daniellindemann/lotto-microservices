import { TestBed } from '@angular/core/testing';

import { LottoNumberService } from './lotto-number.service';

describe('LottoNumberService', () => {
  let service: LottoNumberService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LottoNumberService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
