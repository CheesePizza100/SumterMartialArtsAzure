export interface Instructor {
  id: number;
  name: string;
  rank: string;
  bio: string;
  photoUrl: string;
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
