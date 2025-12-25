export interface Program {
  id: number;
  name: string;
  description: string;
  ageGroup: string;
  details: string;
  duration: string;
  schedule: string;
  imageUrl: string;
  instructors: Instructor[];
}
export interface Instructor {
  id: number;
  name: string;
  rank: string;
  bio: string;
  photoUrl: string;
  programIds: number[];
  achievements: string[];
}

export type ProgramCategory = 'all' | 'kids' | 'adult' | 'competition';
