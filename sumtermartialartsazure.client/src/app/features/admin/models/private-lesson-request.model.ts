export interface PrivateLessonRequest {
  id: number;
  instructorId: number;
  instructorName: string;
  studentName: string;
  studentEmail: string;
  studentPhone: string;
  requestedStart: string;
  requestedEnd: string;
  status: string;
  notes?: string;
  rejectionReason?: string;
  createdAt: string;
}

export interface GetAllPrivateLessons {
  requests: PrivateLessonRequest[];
}

export interface UpdateStatus {
  success: boolean;
  message?: string;
}
