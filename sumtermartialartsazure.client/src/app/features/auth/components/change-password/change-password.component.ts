import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../../core/services/auth.service';

@Component({
  selector: 'app-change-password',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent {
  currentPassword = '';
  newPassword = '';
  confirmPassword = '';
  loading = false;
  error = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  isValid(): boolean {
    return this.currentPassword.length > 0 &&
      this.newPassword.length >= 8 &&
      this.newPassword === this.confirmPassword;
  }

  onSubmit(): void {
    if (!this.isValid()) {
      this.error = 'Please fill in all fields correctly';
      return;
    }

    if (this.newPassword !== this.confirmPassword) {
      this.error = 'New passwords do not match';
      return;
    }

    this.loading = true;
    this.error = '';

    this.authService.changePassword(this.currentPassword, this.newPassword)
      .subscribe({
        next: () => {
          this.loading = false;

          // Get user role and redirect appropriately
          const user = this.authService.getCurrentUser();
          if (user?.role === 'Admin') {
            this.router.navigate(['/admin']);
          } else if (user?.role === 'Student') {
            this.router.navigate(['/student']);
          } else {
            this.router.navigate(['/home']);
          }
        },
        error: (err) => {
          this.loading = false;
          this.error = err.error?.message || 'Failed to change password. Please check your current password.';
          console.error('Error changing password:', err);
        }
      });
  }
}
