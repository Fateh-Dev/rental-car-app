import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'appCurrency',
  standalone: true,
  pure: false
})
export class AppCurrencyPipe implements PipeTransform {
  transform(value: number | string | null | undefined): string {
    if (value == null) return '';
    const num = typeof value === 'string' ? parseFloat(value) : value;
    if (isNaN(num)) return '';
    
    // Get symbol from localStorage or default to DZD
    const symbol = localStorage.getItem('parc_auto_currency') || 'DZD';
    
    // Format number (e.g. 1 500,00)
    const formatted = new Intl.NumberFormat('fr-FR', {
      minimumFractionDigits: 0,
      maximumFractionDigits: 2
    }).format(num);

    return `${formatted} ${symbol}`;
  }
}
