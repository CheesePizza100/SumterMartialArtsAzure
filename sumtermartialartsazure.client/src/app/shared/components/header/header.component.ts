import { Component, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  mobileMenuOpen = false;
  adminDropdownOpen = false;

  // Navigation links
  navLinks = [
    { path: '', label: 'Home', exact: true },
    { path: '/programs', label: 'Programs', exact: false },
    { path: '/about', label: 'About Us', exact: false }
  ];

  // Auth links (shown when not logged in)
  authLinks = [
    { path: '/login', label: 'Login' },
    { path: '/register', label: 'Register' },
  ];

  // Admin dropdown links
  adminLinks = [
    { path: '/admin/private-lessons', label: 'Private Lessons' },
    { path: '/admin/students', label: 'Students' },
    { path: '/admin/analytics', label: 'Analytics' }
  ];

  constructor(
    public auth: AuthService,
    private router: Router
  ) { }

  // Close dropdown when clicking outside
  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    const target = event.target as HTMLElement;
    if (!target.closest('.admin-dropdown-container')) {
      this.adminDropdownOpen = false;
    }
  }

  toggleMobile(): void {
    this.mobileMenuOpen = !this.mobileMenuOpen;
  }

  closeMobileMenu(): void {
    this.mobileMenuOpen = false;
  }

  toggleAdminDropdown(): void {
    this.adminDropdownOpen = !this.adminDropdownOpen;
  }

  closeAdminDropdown(): void {
    this.adminDropdownOpen = false;
  }

  logout(): void {
    this.auth.logout();
    this.closeMobileMenu();
    this.router.navigate(['/']);
  }

  get isAdmin(): boolean {
    return this.auth.isAdmin();
  }
}
