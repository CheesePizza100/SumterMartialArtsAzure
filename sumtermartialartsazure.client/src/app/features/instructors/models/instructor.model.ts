export interface Instructor {
  id: number;
  name: string;
  email?: string;  // Only in admin context
  rank: string;
  bio: string;
  photoUrl: string;
  hasLogin?: boolean;  // Only in admin context
  programIds?: number[];
  achievements?: string[];
  specialties?: string[];
  yearsOfExperience?: number;
}

export interface InstructorWithPrograms extends Instructor {
  programs: ProgramSummary[];
}
export interface ProgramSummary {
  id: number;
  name: string;
}

export const BELT_RANKS = [
  'White',
  'Yellow',
  'Green',
  'Blue',
  'Brown',
  'Black'
] as const;

export type BeltRank = typeof BELT_RANKS[number];

export interface InstructorProfile {
  id: number;
  name: string;
  email: string;
  rank: string;
  bio: string;
  photoUrl: string;
  programs: Program[];
  classSchedule: ClassSchedule[];
}

export interface Program {
  id: number;
  name: string;
}

export interface ClassSchedule {
  daysOfWeek: string;
  startTime: string;
  duration: string;
}

export interface InstructorStudent {
  id: number;
  name: string;
  email: string;
  phone: string;
  programs: StudentProgram[];
  testHistory: TestHistory[];
}

export interface StudentProgram {
  programId: number;
  programName: string;
  currentRank: string;
  enrolledDate: string;
  lastTestDate?: string;
  instructorNotes?: string;
  attendance: Attendance;
}

export interface Attendance {
  last30Days: number;
  total: number;
  attendanceRate: number;
}

export interface TestHistory {
  date: string;
  program: string;
  rank: string;
  result: string;
  notes?: string;
}
