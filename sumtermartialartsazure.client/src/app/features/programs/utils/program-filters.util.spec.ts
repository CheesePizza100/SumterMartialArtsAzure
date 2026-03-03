import { ProgramFilters } from './program-filters.util';
import { Program } from '../models/program.model';

const makeProgram = (id: number, ageGroup: string): Program => ({
  id,
  name: `Program ${id}`,
  description: '',
  ageGroup,
  details: '',
  duration: '',
  schedule: '',
  imageUrl: '',
  instructors: []
});

const mockPrograms: Program[] = [
  makeProgram(1, 'Kids 4-12'),
  makeProgram(2, 'Kids 4–12'),
  makeProgram(3, 'Kids Beginner'),
  makeProgram(4, 'Adult'),
  makeProgram(5, 'Teens and Adults'),
  makeProgram(6, 'Advanced Competition'),
  makeProgram(7, 'Competition Team'),
  makeProgram(8, 'General')
];

describe('ProgramFilters', () => {

  describe('mapAgeGroupToCategory', () => {
    it('should return kids for "4-12" pattern', () => {
      expect(ProgramFilters.mapAgeGroupToCategory('Kids 4-12')).toBe('kids');
    });

    it('should return kids for "4–12" pattern (en dash)', () => {
      expect(ProgramFilters.mapAgeGroupToCategory('Ages 4–12')).toBe('kids');
    });

    it('should return kids for "kids" keyword', () => {
      expect(ProgramFilters.mapAgeGroupToCategory('Kids Beginner')).toBe('kids');
      expect(ProgramFilters.mapAgeGroupToCategory('KIDS')).toBe('kids');
    });

    it('should return adult for "adult" keyword', () => {
      expect(ProgramFilters.mapAgeGroupToCategory('Adult')).toBe('adult');
      expect(ProgramFilters.mapAgeGroupToCategory('Adults Only')).toBe('adult');
    });

    it('should return adult for "teens" keyword', () => {
      expect(ProgramFilters.mapAgeGroupToCategory('Teens')).toBe('adult');
      expect(ProgramFilters.mapAgeGroupToCategory('Teens and Adults')).toBe('adult');
    });

    it('should return competition for "competition" keyword', () => {
      expect(ProgramFilters.mapAgeGroupToCategory('Competition Team')).toBe('competition');
      expect(ProgramFilters.mapAgeGroupToCategory('COMPETITION')).toBe('competition');
    });

    it('should return competition for "advanced" keyword', () => {
      expect(ProgramFilters.mapAgeGroupToCategory('Advanced Competition')).toBe('competition');
      expect(ProgramFilters.mapAgeGroupToCategory('Advanced')).toBe('competition');
    });

    it('should return all for unrecognized age group', () => {
      expect(ProgramFilters.mapAgeGroupToCategory('General')).toBe('all');
      expect(ProgramFilters.mapAgeGroupToCategory('')).toBe('all');
      expect(ProgramFilters.mapAgeGroupToCategory('Unknown')).toBe('all');
    });
  });

  describe('filterByCategory', () => {
    it('should return all programs for all category', () => {
      expect(ProgramFilters.filterByCategory(mockPrograms, 'all')).toEqual(mockPrograms);
    });

    it('should return empty array when no programs match', () => {
      const programs = [makeProgram(1, 'General')];
      expect(ProgramFilters.filterByCategory(programs, 'kids')).toEqual([]);
    });

    it('should filter to kids programs', () => {
      const result = ProgramFilters.filterByCategory(mockPrograms, 'kids');
      expect(result.length).toBe(3);
      expect(result.every(p => ProgramFilters.mapAgeGroupToCategory(p.ageGroup) === 'kids')).toBeTrue();
    });

    it('should filter to adult programs', () => {
      const result = ProgramFilters.filterByCategory(mockPrograms, 'adult');
      expect(result.length).toBe(2);
      expect(result.every(p => ProgramFilters.mapAgeGroupToCategory(p.ageGroup) === 'adult')).toBeTrue();
    });

    it('should filter to competition programs', () => {
      const result = ProgramFilters.filterByCategory(mockPrograms, 'competition');
      expect(result.length).toBe(2);
      expect(result.every(p => ProgramFilters.mapAgeGroupToCategory(p.ageGroup) === 'competition')).toBeTrue();
    });

    it('should return empty array for empty programs list', () => {
      expect(ProgramFilters.filterByCategory([], 'kids')).toEqual([]);
    });
  });
});
