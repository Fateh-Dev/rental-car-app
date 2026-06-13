import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

export interface ConfirmOptions {
  title: string;
  message: string;
  type?: 'danger' | 'warning' | 'info';
  confirmText?: string;
  cancelText?: string;
  icon?: string;
  isAlert?: boolean;
}

interface ConfirmRequest {
  options: ConfirmOptions;
  resolve: (value: boolean) => void;
}

@Injectable({ providedIn: 'root' })
export class ConfirmDialogService {
  private requestSubject = new Subject<ConfirmRequest>();
  readonly request$ = this.requestSubject.asObservable();

  confirm(options: ConfirmOptions): Promise<boolean> {
    return new Promise((resolve) => {
      this.requestSubject.next({ options, resolve });
    });
  }

  alert(options: ConfirmOptions): Promise<void> {
    return new Promise((resolve) => {
      this.requestSubject.next({
        options: { ...options, isAlert: true },
        resolve: () => resolve()
      });
    });
  }
}
