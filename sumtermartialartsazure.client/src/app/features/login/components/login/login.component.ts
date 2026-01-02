import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../../../app/core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username = '';
  password = '';
  loading = false;
  error = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  onSubmit(): void {
    this.loading = true;
    this.error = '';

    this.authService.login(this.username, this.password)
      .subscribe({
        next: () => {
          this.loading = false;
          this.router.navigate(['/admin']);
        },
        error: (err) => {
          this.loading = false;
          this.error = 'Invalid username or password';
          console.error('Login error:', err);
        }
      });
  }
}
