export interface Program {
  name: string;
  rank: string;
  enrolledDate: string;
  lastTest: string | null;
  testNotes: string | null;
}

export interface TestHistory {
  date: string;
  program: string;
  rank: string;
  result: 'Pass' | 'Fail';
  notes: string;
}

export interface Attendance {
  last30Days: number;
  total: number;
  attendanceRate: number;
}

export interface Student {
  id: number;
  name: string;
  email: string;
  phone: string;
  programs: Program[];
  attendance: Attendance;
  testHistory: TestHistory[];
}
