import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTabsModule } from '@angular/material/tabs';
import { MatChipsModule } from '@angular/material/chips';
import { FormsModule } from '@angular/forms';
import { Program, TestHistory, Attendance, Student } from '../../models/student.model'

@Component({
  selector: 'app-admin-students',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    MatProgressSpinnerModule,
    MatTabsModule,
    MatChipsModule,
    FormsModule
  ],
  templateUrl: './admin-students.component.html',
  styleUrls: ['./admin-students.component.css']
})
export class AdminStudentsComponent implements OnInit {
  students: Student[] = [];
  filteredStudents: Student[] = [];
  selectedStudent: Student | null = null;
  searchTerm = '';
  isLoading = true;
  activeTab = 0;

  displayedColumns: string[] = [
    'name',
    'contact',
    'programs',
    'attendance',
    'lastTest'
  ];

  // Sample data - replace with actual service call
  private sampleData: Student[] = [
    {
      id: 1,
      name: "Sarah Johnson",
      email: "sarah.j@email.com",
      phone: "(555) 123-4567",
      programs: [
        {
          name: "Brazilian Jiu-Jitsu",
          rank: "Blue Belt",
          enrolledDate: "2022-03-15",
          lastTest: "2024-08-20",
          testNotes: "Excellent technique on guard passes. Ready for purple belt within 6-8 months with continued progress."
        },
        {
          name: "Kickboxing",
          rank: "Intermediate",
          enrolledDate: "2023-01-10",
          lastTest: null,
          testNotes: null
        }
      ],
      attendance: {
        last30Days: 18,
        total: 156,
        attendanceRate: 85
      },
      testHistory: [
        {
          date: "2024-08-20",
          program: "Brazilian Jiu-Jitsu",
          rank: "Blue Belt",
          result: "Pass",
          notes: "Strong performance in sparring and technique demonstration"
        },
        {
          date: "2023-10-15",
          program: "Brazilian Jiu-Jitsu",
          rank: "Purple Stripe 4",
          result: "Pass",
          notes: "Good progress on submissions"
        },
        {
          date: "2023-06-10",
          program: "Brazilian Jiu-Jitsu",
          rank: "Purple Stripe 3",
          result: "Pass",
          notes: "Excellent fundamentals"
        }
      ]
    },
    {
      id: 2,
      name: "Marcus Chen",
      email: "mchen@email.com",
      phone: "(555) 234-5678",
      programs: [
        {
          name: "Muay Thai",
          rank: "Advanced",
          enrolledDate: "2021-06-01",
          lastTest: "2024-11-05",
          testNotes: "Exceptional striking. Clinch work needs refinement."
        }
      ],
      attendance: {
        last30Days: 24,
        total: 312,
        attendanceRate: 92
      },
      testHistory: [
        {
          date: "2024-11-05",
          program: "Muay Thai",
          rank: "Advanced",
          result: "Pass",
          notes: "Ready for instructor training program"
        },
        {
          date: "2024-03-20",
          program: "Muay Thai",
          rank: "Intermediate Level 3",
          result: "Pass",
          notes: "Excellent power generation"
        }
      ]
    },
    {
      id: 3,
      name: "Emma Rodriguez",
      email: "e.rodriguez@email.com",
      phone: "(555) 345-6789",
      programs: [
        {
          name: "Brazilian Jiu-Jitsu",
          rank: "White Belt (2 stripes)",
          enrolledDate: "2024-09-01",
          lastTest: null,
          testNotes: null
        }
      ],
      attendance: {
        last30Days: 14,
        total: 42,
        attendanceRate: 78
      },
      testHistory: []
    },
    {
      id: 4,
      name: "David Kim",
      email: "david.kim@email.com",
      phone: "(555) 456-7890",
      programs: [
        {
          name: "Brazilian Jiu-Jitsu",
          rank: "Purple Belt",
          enrolledDate: "2020-01-15",
          lastTest: "2024-06-10",
          testNotes: "Strong guard game. Needs work on passing from top position."
        },
        {
          name: "Wrestling",
          rank: "Intermediate",
          enrolledDate: "2023-08-01",
          lastTest: "2024-10-01",
          testNotes: "Natural talent for takedowns."
        }
      ],
      attendance: {
        last30Days: 20,
        total: 428,
        attendanceRate: 88
      },
      testHistory: [
        {
          date: "2024-10-01",
          program: "Wrestling",
          rank: "Intermediate",
          result: "Pass",
          notes: "Excellent technique refinement"
        },
        {
          date: "2024-06-10",
          program: "Brazilian Jiu-Jitsu",
          rank: "Purple Belt",
          result: "Pass",
          notes: "Solid all-around performance"
        },
        {
          date: "2023-08-15",
          program: "Brazilian Jiu-Jitsu",
          rank: "Blue Belt Stripe 4",
          result: "Pass",
          notes: "Ready for purple belt promotion"
        }
      ]
    },
    {
      id: 5,
      name: "Jessica Patel",
      email: "j.patel@email.com",
      phone: "(555) 567-8901",
      programs: [
        {
          name: "Kickboxing",
          rank: "Beginner",
          enrolledDate: "2024-10-15",
          lastTest: null,
          testNotes: null
        }
      ],
      attendance: {
        last30Days: 8,
        total: 16,
        attendanceRate: 67
      },
      testHistory: []
    }
  ];

  ngOnInit(): void {
    this.loadStudents();
  }

  loadStudents(): void {
    // Simulate API call
    setTimeout(() => {
      this.students = this.sampleData;
      this.filteredStudents = this.students;
      this.isLoading = false;
    }, 500);
  }

  onSearchChange(): void {
    const term = this.searchTerm.toLowerCase();
    this.filteredStudents = this.students.filter(student =>
      student.name.toLowerCase().includes(term) ||
      student.email.toLowerCase().includes(term) ||
      student.programs.some(p => p.name.toLowerCase().includes(term))
    );
  }

  selectStudent(student: Student): void {
    this.selectedStudent = student;
    this.activeTab = 0;
  }

  backToList(): void {
    this.selectedStudent = null;
  }

  formatDate(dateStr: string): string {
    return new Date(dateStr).toLocaleDateString('en-US', {
      month: 'short',
      day: 'numeric',
      year: 'numeric'
    });
  }

  getLastTestDate(student: Student): string {
    if (student.testHistory.length === 0) {
      return 'No tests yet';
    }
    return this.formatDate(student.testHistory[0].date);
  }

  getResultColor(result: string): string {
    return result.toLowerCase() === 'pass' ? 'primary' : 'warn';
  }
}
