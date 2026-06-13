import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ConfirmDialogService, ConfirmOptions } from '../../../services/confirm-dialog.service';
import { Subscription } from 'rxjs';
import { TranslatePipe } from '../../../pipes/translate.pipe';

@Component({
  selector: 'app-confirm-dialog',
  standalone: true,
  imports: [CommonModule, TranslatePipe],
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.css']
})
export class ConfirmDialogComponent implements OnInit, OnDestroy {
  show = false;
  options!: ConfirmOptions;
  private resolveFn!: (value: boolean) => void;
  private sub?: Subscription;

  constructor(private confirmService: ConfirmDialogService) {}

  ngOnInit(): void {
    this.sub = this.confirmService.request$.subscribe(request => {
      this.options = request.options;
      this.resolveFn = request.resolve;
      this.show = true;
    });
  }

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
  }

  confirm(): void {
    this.show = false;
    this.resolveFn(true);
  }

  cancel(): void {
    this.show = false;
    this.resolveFn(false);
  }
}
