import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialog } from '@angular/material/dialog';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatTabsModule } from '@angular/material/tabs';
import { AdminPrivateLessonsService } from '../../services/admin-private-lessons.service';
import { PrivateLessonRequest, UpdateStatus } from '../../models/private-lesson-request.model';
import { RejectionReasonDialogComponent } from '../rejection-reason-dialog/rejection-reason-dialog.component';

@Component({
  selector: 'app-admin-private-lessons',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    MatTooltipModule,
    MatTabsModule
  ],
  templateUrl: './admin-private-lessons.component.html',
  styleUrls: ['./admin-private-lessons.component.css']
})
export class AdminPrivateLessonsComponent implements OnInit {
  requests: PrivateLessonRequest[] = [];
  displayedColumns: string[] = [
    'studentName',
    'instructorName',
    'requestedTime',
    'status',
    'createdAt',
    'actions'
  ];
  isLoading = true;
  error?: string;
  activeTab = 0;
  pendingCount = 0;

  constructor(
    private adminService: AdminPrivateLessonsService,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.loadRequests('Pending');
    this.loadPendingCount();
  }

  loadRequests(filter: string = 'Pending'): void {
    this.isLoading = true;
    this.error = undefined;

    this.adminService.getAllRequests(filter).subscribe({
      next: (requests) => {
        this.requests = requests || [];
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading requests:', err);
        this.error = 'Failed to load requests';
        this.requests = [];
        this.isLoading = false;
      }
    });
  }

  loadPendingCount(): void {
    // Load pending count for the badge
    this.adminService.getAllRequests('Pending').subscribe({
      next: (requests) => {
        this.pendingCount = requests.length;
      },
      error: (err) => {
        console.error('Error loading pending count:', err);
      }
    });
  }

  approve(request: PrivateLessonRequest): void {
    this.adminService.updateStatus(request.id, 'Approved', undefined).subscribe({
      next: (response) => {
        if (response.success) {
          this.snackBar.open('Request approved successfully', 'Close', {
            duration: 3000,
            panelClass: ['success-snackbar']
          });
          const filters = ['Pending', 'Recent', 'All'];
          this.loadRequests(filters[this.activeTab]);
          this.loadPendingCount(); // Refresh badge count
        } else {
          this.snackBar.open(
            response.message || 'Failed to approve request',
            'Close',
            { duration: 5000 }
          );
        }
      },
      error: (err) => {
        console.error('Error approving request:', err);
        this.snackBar.open('Error approving request', 'Close', { duration: 3000 });
      }
    });
  }

  reject(request: PrivateLessonRequest): void {
    const dialogRef = this.dialog.open(RejectionReasonDialogComponent, {
      width: '400px',
      data: { studentName: request.studentName }
    });

    dialogRef.afterClosed().subscribe(reason => {
      if (reason) {
        this.adminService.updateStatus(request.id, 'Rejected', reason).subscribe({
          next: (response) => {
            if (response.success) {
              this.snackBar.open('Request rejected', 'Close', {
                duration: 3000
              });
              const filters = ['Pending', 'Recent', 'All'];
              this.loadRequests(filters[this.activeTab]);
              this.loadPendingCount(); // Refresh badge count
            } else {
              this.snackBar.open(
                response.message || 'Failed to reject request',
                'Close',
                { duration: 5000 }
              );
            }
          },
          error: (err) => {
            console.error('Error rejecting request:', err);
            this.snackBar.open('Error rejecting request', 'Close', { duration: 3000 });
          }
        });
      }
    });
  }

  onTabChange(index: number): void {
    const filters = ['Pending', 'Recent', 'All'];
    this.loadRequests(filters[index]);
  }

  formatDateTime(dateStr: string): string {
    return new Date(dateStr).toLocaleString('en-US', {
      month: 'short',
      day: 'numeric',
      year: 'numeric',
      hour: 'numeric',
      minute: '2-digit'
    });
  }

  formatTimeRange(start: string, end: string): string {
    const startDate = new Date(start);
    const endDate = new Date(end);
    return `${startDate.toLocaleDateString('en-US', {
      month: 'short',
      day: 'numeric'
    })} ${startDate.toLocaleTimeString('en-US', {
      hour: 'numeric',
      minute: '2-digit'
    })} - ${endDate.toLocaleTimeString('en-US', {
      hour: 'numeric',
      minute: '2-digit'
    })}`;
  }

  getStatusColor(status: string): string {
    switch (status.toLowerCase()) {
      case 'pending': return 'accent';
      case 'approved': return 'primary';
      case 'rejected': return 'warn';
      default: return '';
    }
  }

  isPending(request: PrivateLessonRequest): boolean {
    return request.status.toLowerCase() === 'pending';
  }
}
