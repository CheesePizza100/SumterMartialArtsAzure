import { Component } from '@angular/core';
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
    { path: '/admin', label: 'Admin' }
  ];

  constructor(
    public auth: AuthService,
    private router: Router
  ) { }

  toggleMobile(): void {
    this.mobileMenuOpen = !this.mobileMenuOpen;
  }

  closeMobileMenu(): void {
    this.mobileMenuOpen = false;
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
