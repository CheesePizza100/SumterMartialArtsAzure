import { inject } from '@angular/core';
import { Router, CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const passwordChangeGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const user = authService.getCurrentUser();

  // If user must change password, redirect to change password page
  if (user?.mustChangePassword) {
    router.navigate(['/change-password']);
    return false;
  }

  return true;
};
