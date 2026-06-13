import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { DialogModule } from 'primeng/dialog';
import { I18nService } from '../../services/i18n.service';
import { TranslatePipe } from '../../pipes/translate.pipe';
import { ConfirmDialogService } from '../../services/confirm-dialog.service';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule, FormsModule, DialogModule, TranslatePipe],
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent implements OnInit {
  users: any[] = [];
  totalCount = 0;
  page = 1;
  pageSize = 10;
  pages: number[] = [];
  searchQuery = '';
  loading = false;

  currentUserId: number | null = null;
  currentUsername = '';

  // CRUD Dialog States
  showCrudDialog = false;
  isEditMode = false;
  userForm: any = this.getEmptyForm();

  // Notification Banners
  successMessage = '';
  errorMessage = '';

  constructor(private api: ApiService, public i18n: I18nService, private confirmService: ConfirmDialogService) {}

  ngOnInit(): void {
    const userJson = localStorage.getItem('parc_auto_user');
    if (userJson) {
      const user = JSON.parse(userJson);
      this.currentUserId = user.id || null;
      this.currentUsername = user.username || '';
    }
    this.loadUsers();
  }

  loadUsers(): void {
    this.loading = true;
    this.api.getUsers(this.searchQuery, this.page, this.pageSize).subscribe({
      next: (res) => {
        this.users = res.data;
        this.totalCount = res.totalCount;
        this.updatePagesArray();
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load users', err);
        this.showError(this.i18n.t('common.errorOccurred'));
        this.loading = false;
      }
    });
  }

  onSearchChange(): void {
    this.page = 1;
    this.loadUsers();
  }

  onPageChange(page: number): void {
    this.page = page;
    this.loadUsers();
  }

  updatePagesArray(): void {
    const totalPages = Math.ceil(this.totalCount / this.pageSize);
    this.pages = [];
    for (let i = 1; i <= totalPages; i++) {
      this.pages.push(i);
    }
  }

  getEmptyForm(): any {
    return {
      id: 0,
      username: '',
      fullName: '',
      password: '',
      confirmPassword: '',
      isLocked: false
    };
  }

  openAddDialog(): void {
    this.isEditMode = false;
    this.userForm = this.getEmptyForm();
    this.showCrudDialog = true;
    this.clearMessages();
  }

  openEditDialog(user: any): void {
    this.isEditMode = true;
    this.userForm = {
      id: user.id,
      username: user.username,
      fullName: user.fullName,
      password: '',
      confirmPassword: '',
      isLocked: user.isLocked
    };
    this.showCrudDialog = true;
    this.clearMessages();
  }

  toggleLock(user: any): void {
    if (user.id === this.currentUserId) {
      this.showError(this.i18n.t('users.selfLockError'));
      return;
    }

    this.confirmService.confirm({
      title: user.isLocked ? this.i18n.t('users.unlockUser') : this.i18n.t('users.lockUser'),
      message: user.isLocked ? this.i18n.t('users.unlockConfirm') : this.i18n.t('users.lockConfirm'),
      type: user.isLocked ? 'info' : 'warning',
      icon: user.isLocked ? 'pi pi-unlock' : 'pi pi-lock'
    }).then(confirmed => {
      if (confirmed) {
        this.api.toggleUserLock(user.id).subscribe({
          next: (res) => {
            user.isLocked = res.isLocked;
            this.showSuccess(res.isLocked ? this.i18n.t('users.lockUserSuccess') : this.i18n.t('users.unlockUserSuccess'));
            this.loadUsers();
          },
          error: (err) => {
            console.error(err);
            this.showError(err.error?.message || this.i18n.t('users.errorTogglingLock'));
          }
        });
      }
    });
  }

  deleteUser(id: number): void {
    if (id === this.currentUserId) {
      this.showError(this.i18n.t('users.selfDeleteError'));
      return;
    }

    this.confirmService.confirm({
      title: this.i18n.t('common.delete'),
      message: this.i18n.t('users.deleteConfirm'),
      type: 'danger',
      icon: 'pi pi-trash'
    }).then(confirmed => {
      if (confirmed) {
        this.api.deleteUser(id).subscribe({
          next: () => {
            this.showSuccess(this.i18n.t('users.deleteUserSuccess'));
            this.loadUsers();
          },
          error: (err) => {
            console.error(err);
            this.showError(err.error?.message || this.i18n.t('users.errorDeletingUser'));
          }
        });
      }
    });
  }

  onSubmitUser(): void {
    this.clearMessages();

    // Validation
    if (!this.userForm.username.trim()) {
      this.showError(this.i18n.t('users.usernameRequired'));
      return;
    }
    if (!this.userForm.fullName.trim()) {
      this.showError(this.i18n.t('users.fullNameRequired'));
      return;
    }

    if (!this.isEditMode && !this.userForm.password) {
      this.showError(this.i18n.t('users.passwordRequired'));
      return;
    }

    if (this.userForm.password) {
      if (this.userForm.password.length < 6) {
        this.showError(this.i18n.t('users.passwordMinLength'));
        return;
      }
      if (this.userForm.password !== this.userForm.confirmPassword) {
        this.showError(this.i18n.t('users.passwordMismatch'));
        return;
      }
    }

    const payload = {
      username: this.userForm.username,
      fullName: this.userForm.fullName,
      password: this.userForm.password || null
    };

    if (this.isEditMode) {
      this.api.updateUser(this.userForm.id, payload).subscribe({
        next: () => {
          this.showCrudDialog = false;
          this.showSuccess(this.i18n.t('users.updateUserSuccess'));
          this.loadUsers();
        },
        error: (err) => {
          console.error(err);
          this.showError(err.error?.message || this.i18n.t('users.errorUpdatingUser'));
        }
      });
    } else {
      this.api.createUser(payload).subscribe({
        next: () => {
          this.showCrudDialog = false;
          this.showSuccess(this.i18n.t('users.createUserSuccess'));
          this.loadUsers();
        },
        error: (err) => {
          console.error(err);
          this.showError(err.error?.message || this.i18n.t('users.errorCreatingUser'));
        }
      });
    }
  }

  getInitials(fullName: string): string {
    if (!fullName) return '?';
    const parts = fullName.trim().split(' ');
    if (parts.length >= 2) {
      return (parts[0].charAt(0) + parts[1].charAt(0)).toUpperCase();
    }
    return fullName.charAt(0).toUpperCase();
  }

  private showSuccess(msg: string): void {
    this.successMessage = msg;
    setTimeout(() => this.successMessage = '', 5000);
  }

  private showError(msg: string): void {
    this.errorMessage = msg;
    setTimeout(() => this.errorMessage = '', 5000);
  }

  private clearMessages(): void {
    this.successMessage = '';
    this.errorMessage = '';
  }
}
