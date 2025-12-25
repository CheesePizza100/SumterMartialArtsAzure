export interface PrivateLessonRequest {
  id?: number;
  instructorId: number;
  studentName: string;
  studentEmail: string;
  studentPhone?: string;
  requestedStart: string; // ISO date string
  requestedEnd: string; // ISO date string
  notes?: string;
  status?: PrivateLessonStatus;
  createdAt?: string;
}

export enum PrivateLessonStatus {
  Pending = 'pending',
  Approved = 'approved',
  Rejected = 'rejected',
  Completed = 'completed'
}

export interface PrivateLessonDialogData {
  instructorName: string;
  instructorId: number;
}

export interface InstructorAvailability {
  instructorId: number;
  availableDates: Date[];
}
export interface LessonTime {
  start: string;
  end: string;
  durationMinutes?: number;
}
