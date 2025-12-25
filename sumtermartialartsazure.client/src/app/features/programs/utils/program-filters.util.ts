import { Program, ProgramCategory } from '../models/program.model';

export class ProgramFilters {
  static filterByCategory(programs: Program[], category: ProgramCategory): Program[] {
    if (category === 'all') {
      return programs;
    }
    return programs.filter(p => this.mapAgeGroupToCategory(p.ageGroup) === category);
  }

  static mapAgeGroupToCategory(ageGroup: string): ProgramCategory {
    const normalized = ageGroup.toLowerCase();

    if (normalized.includes('4–12') || normalized.includes('4-12') || normalized.includes('kids')) {
      return 'kids';
    }
    if (normalized.includes('teens') || normalized.includes('adult')) {
      return 'adult';
    }
    if (normalized.includes('advanced') || normalized.includes('competition')) {
      return 'competition';
    }
    return 'all';
  }
}
