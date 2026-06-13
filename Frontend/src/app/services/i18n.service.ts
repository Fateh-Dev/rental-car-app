import { Injectable, signal, computed } from '@angular/core';
import { FR } from '../i18n/fr';
import { EN } from '../i18n/en';
import { AR } from '../i18n/ar';

export type Lang = 'fr' | 'en' | 'ar';

const TRANSLATIONS: Record<Lang, Record<string, any>> = {
  fr: FR,
  en: EN,
  ar: AR
};

const STORAGE_KEY = 'parc_auto_lang';

@Injectable({ providedIn: 'root' })
export class I18nService {
  private _lang = signal<Lang>(this.getInitialLang());

  /** Reactive current language */
  readonly currentLang = this._lang.asReadonly();

  /** Whether the current language is RTL */
  readonly isRtl = computed(() => this._lang() === 'ar');

  constructor() {
    this.applyDirection(this._lang());
  }

  /** Switch the active language */
  setLang(lang: Lang): void {
    this._lang.set(lang);
    localStorage.setItem(STORAGE_KEY, lang);
    this.applyDirection(lang);
  }

  /** Translate a key using dot-notation (e.g. 'sidebar.dashboard') */
  t(key: string): string {
    const lang = this._lang();
    const parts = key.split('.');
    let value: any = TRANSLATIONS[lang];

    for (const part of parts) {
      if (value && typeof value === 'object' && part in value) {
        value = value[part];
      } else {
        // Fallback to French, then return the key itself
        let fallback: any = TRANSLATIONS['fr'];
        for (const p of parts) {
          if (fallback && typeof fallback === 'object' && p in fallback) {
            fallback = fallback[p];
          } else {
            return key;
          }
        }
        return typeof fallback === 'string' ? fallback : key;
      }
    }

    return typeof value === 'string' ? value : key;
  }

  /** Dynamically translate alert messages returned from the backend in English */
  translateAlertMessage(msg: string): string {
    const lang = this._lang();
    if (lang === 'en') return msg;

    try {
      // 1. Insurance expired
      if (msg.startsWith('Insurance expired on')) {
        const date = msg.replace('Insurance expired on ', '').replace('.', '').trim();
        if (lang === 'fr') return `Assurance expirée le ${date}.`;
        if (lang === 'ar') return `انتهت صلاحية التأمين بتاريخ ${date}.`;
      }
      // "Insurance expiring soon in 5 days."
      if (msg.startsWith('Insurance expiring soon in')) {
        const match = msg.match(/\d+/);
        const days = match ? match[0] : '';
        if (lang === 'fr') return `L'assurance expire bientôt dans ${days} jours.`;
        if (lang === 'ar') return `ينتهي التأمين قريباً خلال ${days} أيام.`;
      }

      // 2. Technical inspection failed
      if (msg === 'Technical inspection failed! Vehicle requires repairs.') {
        if (lang === 'fr') return `Le contrôle technique a échoué ! Le véhicule nécessite des réparations.`;
        if (lang === 'ar') return `فشل الفحص الفني! المركبة تتطلب إصلاحات.`;
      }
      // "Technical inspection expired on 2026-06-04."
      if (msg.startsWith('Technical inspection expired on')) {
        const date = msg.replace('Technical inspection expired on ', '').replace('.', '').trim();
        if (lang === 'fr') return `Contrôle technique expiré le ${date}.`;
        if (lang === 'ar') return `انتهت صلاحية الفحص الفني بتاريخ ${date}.`;
      }
      // "Technical inspection expiring in 5 days."
      if (msg.startsWith('Technical inspection expiring in')) {
        const match = msg.match(/\d+/);
        const days = match ? match[0] : '';
        if (lang === 'fr') return `Le contrôle technique expire dans ${days} jours.`;
        if (lang === 'ar') return `ينتهي الفحص الفني خلال ${days} أيام.`;
      }

      // 3. Maintenance due
      if (msg.startsWith('Scheduled ') && msg.includes(' is overdue by ')) {
        const parts = msg.replace('Scheduled ', '').split(' is overdue by ');
        const type = parts[0];
        const daysMatch = parts[1] ? parts[1].match(/\d+/) : null;
        const days = daysMatch ? daysMatch[0] : '';
        const translatedType = this.translateMaintenanceType(type);
        if (lang === 'fr') return `La maintenance planifiée (${translatedType}) est en retard de ${days} jours.`;
        if (lang === 'ar') return `الصيانة المجدولة (${translatedType}) متأخرة بـ ${days} أيام.`;
      }
      if (msg.startsWith('Upcoming scheduled ') && msg.includes(' in ')) {
        const parts = msg.replace('Upcoming scheduled ', '').split(' in ');
        const type = parts[0];
        const daysMatch = parts[1] ? parts[1].match(/\d+/) : null;
        const days = daysMatch ? daysMatch[0] : '';
        const translatedType = this.translateMaintenanceType(type);
        if (lang === 'fr') return `Maintenance planifiée (${translatedType}) à venir dans ${days} jours.`;
        if (lang === 'ar') return `الصيانة المجدولة القادمة (${translatedType}) خلال ${days} أيام.`;
      }

      // 4. Odometer inactivity
      if (msg.startsWith('No mileage activity registered for')) {
        const match = msg.match(/\d+/);
        const days = match ? match[0] : '';
        if (lang === 'fr') return `Aucune activité kilométrique enregistrée depuis ${days} jours.`;
        if (lang === 'ar') return `لم يتم تسجيل أي نشاط للمسافة المقطوعة منذ ${days} أيام.`;
      }

      // 5. Driver license
      if (msg.startsWith("Driver's license expired on")) {
        const date = msg.replace("Driver's license expired on ", '').replace('.', '').trim();
        if (lang === 'fr') return `Le permis de conduire a expiré le ${date}.`;
        if (lang === 'ar') return `انتهت صلاحية رخصة القيادة بتاريخ ${date}.`;
      }
      if (msg.startsWith("Driver's license expiring in")) {
        const match = msg.match(/\d+/);
        const days = match ? match[0] : '';
        if (lang === 'fr') return `Le permis de conduire expire dans ${days} jours.`;
        if (lang === 'ar') return `تنتهي صلاحية رخصة القيادة خلال ${days} أيام.`;
      }

      // 6. Consumables
      if (msg.includes('replacement is overdue.')) {
        const matchType = msg.match(/^([a-zA-Z]+) replacement/);
        const type = matchType ? matchType[1] : 'Consumable';
        const parsedType = this.translateConsumableType(type);
        const matches = msg.match(/\d+/g);
        if (matches && matches.length >= 4) {
          const [intervalKm, intervalMonths, kmSince, monthsSince] = matches;
          if (lang === 'fr') return `Le remplacement de ${parsedType} est dépassé. Intervalle: ${intervalKm} km / ${intervalMonths} mois. Actuel: ${kmSince} km / ${monthsSince} mois.`;
          if (lang === 'ar') return `تجاوز موعد استبدال ${parsedType}. الفاصل الزمني: ${intervalKm} كم / ${intervalMonths} شهر. الحالي: ${kmSince} كم / ${monthsSince} شهر.`;
        }
      }
      if (msg.includes('replacement approaching due threshold.')) {
        const matchType = msg.match(/^([a-zA-Z]+) replacement/);
        const type = matchType ? matchType[1] : 'Consumable';
        const parsedType = this.translateConsumableType(type);
        const matches = msg.match(/\d+/g);
        if (matches && matches.length >= 2) {
          const [kmSince, monthsSince] = matches;
          if (lang === 'fr') return `Le remplacement de ${parsedType} approche de l'échéance. Actuel: ${kmSince} km / ${monthsSince} mois.`;
          if (lang === 'ar') return `يقترب موعد استبدال ${parsedType}. الحالي: ${kmSince} كم / ${monthsSince} شهر.`;
        }
      }
    } catch (e) {
      console.warn('Failed to parse alert message translation: ', msg, e);
    }

    return msg;
  }

  translateValidationError(msg: string): string {
    const lang = this._lang();
    if (lang === 'en') return msg;

    try {
      // 1. "The {0} field is required."
      const requiredMatch = msg.match(/^The (.+) field is required\.$/);
      if (requiredMatch) {
        const field = requiredMatch[1];
        if (lang === 'fr') return `Le champ ${field} est obligatoire.`;
        if (lang === 'ar') return `حقل ${field} مطلوب.`;
      }
      
      // 2. "The field {0} must be a string or array type with a maximum length of '{1}'."
      const maxLengthMatch = msg.match(/^The field (.+) must be a string or array type with a maximum length of '(\d+)'\.$/);
      if (maxLengthMatch) {
        const field = maxLengthMatch[1];
        const length = maxLengthMatch[2];
        if (lang === 'fr') return `Le champ ${field} ne doit pas dépasser ${length} caractères.`;
        if (lang === 'ar') return `يجب ألا يتجاوز حقل ${field} ${length} حرفاً.`;
      }

      // 3. "The {0} field is not a valid e-mail address."
      const emailMatch = msg.match(/^The (.+) field is not a valid e-mail address\.$/);
      if (emailMatch) {
        const field = emailMatch[1];
        if (lang === 'fr') return `Le champ ${field} n'est pas une adresse e-mail valide.`;
        if (lang === 'ar') return `حقل ${field} ليس عنوان بريد إلكتروني صالح.`;
      }

      // Add common backend error messages translation
      if (msg === 'One or more validation errors occurred.') {
        if (lang === 'fr') return 'Une ou plusieurs erreurs de validation se sont produites.';
        if (lang === 'ar') return 'حدث خطأ واحد أو أكثر في التحقق من الصحة.';
      }
    } catch (e) {
      console.warn('Failed to parse validation error translation: ', msg, e);
    }
    
    return msg;
  }

  translateMaintenanceType(type: string): string {
    const lang = this._lang();
    const key = type.toLowerCase();
    if (lang === 'fr') {
      if (key === 'preventive') return 'Préventif';
      if (key === 'corrective') return 'Correctif';
      if (key === 'inspection') return 'Inspection';
      if (key === 'accidentrepair') return 'Réparation accident';
    }
    if (lang === 'ar') {
      if (key === 'preventive') return 'وقائي';
      if (key === 'corrective') return 'تصحيحي';
      if (key === 'inspection') return 'فحص';
      if (key === 'accidentrepair') return 'إsلاح حادث';
    }
    return type;
  }

  translateConsumableType(type: string): string {
    const lang = this._lang();
    const key = type.toLowerCase();
    if (lang === 'fr') {
      if (key === 'oilchange') return "Vidange d'huile";
      if (key === 'airfilter') return 'Filtre à air';
      if (key === 'oilfilter') return 'Filtre à huile';
      if (key === 'fuelfilter') return 'Filtre à carburant';
      if (key === 'cabinfilter') return "Filtre d'habitacle";
      if (key === 'frontbrakes') return 'Freins avant';
      if (key === 'rearbrakes') return 'Freins arrière';
      if (key === 'fronttires') return 'Pneus avant';
      if (key === 'reartires') return 'Pneus arrière';
      if (key === 'battery') return 'Batterie';
    }
    if (lang === 'ar') {
      if (key === 'oilchange') return 'تغيير الزيت';
      if (key === 'airfilter') return 'فلتر الهواء';
      if (key === 'oilfilter') return 'فلتر الزيت';
      if (key === 'fuelfilter') return 'فلتر الوقود';
      if (key === 'cabinfilter') return 'فلتر المقصورة';
      if (key === 'frontbrakes') return 'الفرامل الأمامية';
      if (key === 'rearbrakes') return 'الفرامل الخلفية';
      if (key === 'fronttires') return 'الإطارات الأمامية';
      if (key === 'reartires') return 'الإطارات الخلفية';
      if (key === 'battery') return 'البطارية';
    }
    return type;
  }

  /** Dynamically translate alert time strings returned from the backend in English */
  translateAlertTime(timeStr: string): string {
    const lang = this._lang();
    if (lang === 'en') return timeStr;

    // "FAILED"
    if (timeStr === 'FAILED') {
      if (lang === 'fr') return 'ÉCHEC';
      if (lang === 'ar') return 'فشل';
    }

    // "X days overdue"
    if (timeStr.includes('days overdue')) {
      const num = timeStr.match(/\d+/)?.[0] || '';
      if (lang === 'fr') return `${num} j de retard`;
      if (lang === 'ar') return `متأخر بـ ${num} يوم`;
    }

    // "X days remaining"
    if (timeStr.includes('days remaining')) {
      const num = timeStr.match(/\d+/)?.[0] || '';
      if (lang === 'fr') return `${num} j restants`;
      if (lang === 'ar') return `متبقي ${num} يوم`;
    }

    // "X days left"
    if (timeStr.includes('days left')) {
      const num = timeStr.match(/\d+/)?.[0] || '';
      if (lang === 'fr') return `${num} j restants`;
      if (lang === 'ar') return `متبقي ${num} يوم`;
    }

    // "X days inactive"
    if (timeStr.includes('days inactive')) {
      const num = timeStr.match(/\d+/)?.[0] || '';
      if (lang === 'fr') return `${num} j inactif`;
      if (lang === 'ar') return `غير نشط ${num} يوم`;
    }

    // "X km overdue"
    if (timeStr.includes('km overdue')) {
      const num = timeStr.match(/\d+/)?.[0] || '';
      if (lang === 'fr') return `${num} km de retard`;
      if (lang === 'ar') return `تجاوز بـ ${num} كم`;
    }

    // "X km left"
    if (timeStr.includes('km left')) {
      const num = timeStr.match(/\d+/)?.[0] || '';
      if (lang === 'fr') return `${num} km restants`;
      if (lang === 'ar') return `متبقي ${num} كم`;
    }

    // "X months overdue"
    if (timeStr.includes('months overdue')) {
      const num = timeStr.match(/\d+/)?.[0] || '';
      if (lang === 'fr') return `${num} mois de retard`;
      if (lang === 'ar') return `متأخر بـ ${num} شهر`;
    }

    return timeStr;
  }

  /** Get all available languages with labels */
  getLanguages(): { code: Lang; label: string; flag: string }[] {
    return [
      { code: 'fr', label: 'Français', flag: '🇫🇷' },
      { code: 'en', label: 'English', flag: '🇬🇧' },
      { code: 'ar', label: 'العربية', flag: '🇸🇦' }
    ];
  }

  private getInitialLang(): Lang {
    const stored = localStorage.getItem(STORAGE_KEY) as Lang | null;
    if (stored && (stored === 'fr' || stored === 'en' || stored === 'ar')) {
      return stored;
    }
    return 'fr';
  }

  private applyDirection(lang: Lang): void {
    if (typeof document !== 'undefined') {
      const html = document.documentElement;
      if (lang === 'ar') {
        html.setAttribute('dir', 'rtl');
        html.setAttribute('lang', 'ar');
        document.body.classList.add('rtl');
      } else {
        html.setAttribute('dir', 'ltr');
        html.setAttribute('lang', lang);
        document.body.classList.remove('rtl');
      }
    }
  }
}
