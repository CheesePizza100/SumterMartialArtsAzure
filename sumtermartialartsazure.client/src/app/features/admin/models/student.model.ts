export interface Program {
  programId: number;
  name: string;
  rank: string;
  enrolledDate: string;
  lastTest: string | null;
  testNotes: string | null;
  attendance: Attendance;
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
  hasLogin: boolean;
  programs: Program[];
  testHistory: TestHistory[];
}
