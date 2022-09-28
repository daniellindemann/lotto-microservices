import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'leadingZero'
})
export class LeadingZeroPipe implements PipeTransform {
  transform(value: number, cleanLowerThanZero: boolean = true): string {
    if (value <= 0 && cleanLowerThanZero) {
      return '';
    }

    if (value <= 9) {
      return '0' + value;
    }

    return value.toString();
  }
}
