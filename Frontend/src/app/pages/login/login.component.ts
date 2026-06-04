import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { I18nService, Lang } from '../../services/i18n.service';
import { TranslatePipe } from '../../pipes/translate.pipe';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, TranslatePipe],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username = '';
  password = '';
  errorMessage = '';
  isLoading = false;

  languages: { code: Lang; label: string; flag: string }[] = [];

  constructor(private api: ApiService, private router: Router, public i18n: I18nService) {
    this.languages = this.i18n.getLanguages();
    // Redirect if already logged in
    if (localStorage.getItem('parc_auto_token')) {
      this.router.navigate(['/dashboard']);
    }
  }

  switchLang(lang: Lang): void {
    this.i18n.setLang(lang);
  }

  onSubmit(): void {
    if (!this.username || !this.password) {
      this.errorMessage = this.i18n.t('login.errorRequired');
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.api.login({ username: this.username, password: this.password }).subscribe({
      next: (res) => {
        localStorage.setItem('parc_auto_token', res.token);
        localStorage.setItem('parc_auto_user', JSON.stringify(res.user));
        this.router.navigate(['/dashboard']);
      },
      error: (err) => {
        this.isLoading = false;
        if (err.status === 401) {
          this.errorMessage = this.i18n.t('login.errorInvalid');
        } else {
          this.errorMessage = this.i18n.t('login.errorServer');
        }
        console.error('Login error', err);
      }
    });
  }
}
