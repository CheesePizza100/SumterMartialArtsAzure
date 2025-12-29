import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { EventSourcingService } from '../../services/event-sourcing.service';
import { ProgressionAnalytics } from '../../models/event-sourcing.model';

@Component({
  selector: 'app-analytics-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatSelectModule,
    MatFormFieldModule,
    FormsModule
  ],
  templateUrl: './analytics-dashboard.component.html',
  styleUrls: ['./analytics-dashboard.component.css']
})
export class AnalyticsDashboardComponent implements OnInit {
  analytics: ProgressionAnalytics | null = null;
  isLoading = true;
  error?: string;
  selectedProgramId?: number;

  constructor(private eventSourcingService: EventSourcingService) { }

  ngOnInit(): void {
    this.loadAnalytics();
  }

  loadAnalytics(): void {
    this.isLoading = true;
    this.error = undefined;

    this.eventSourcingService.getProgressionAnalytics(this.selectedProgramId).subscribe({
      next: (analytics) => {
        this.analytics = analytics;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading analytics:', err);
        this.error = 'Failed to load analytics. Please try again.';
        this.isLoading = false;
      }
    });
  }

  onProgramFilterChange(): void {
    this.loadAnalytics();
  }

  getMonthName(month: number): string {
    const months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
      'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    return months[month - 1];
  }

  getBarWidth(count: number, maxCount: number): number {
    return maxCount > 0 ? (count / maxCount) * 100 : 0;
  }

  get maxTestCount(): number {
    if (!this.analytics?.mostActiveTestingMonths.length) return 0;
    return Math.max(...this.analytics.mostActiveTestingMonths.map(m => m.testCount));
  }

  get maxRankCount(): number {
    if (!this.analytics?.currentRankDistribution.length) return 0;
    return Math.max(...this.analytics.currentRankDistribution.map(r => r.count));
  }
}
