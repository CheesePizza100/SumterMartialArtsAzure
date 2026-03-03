import { BeltRankUtil } from './belt-rank.util';

describe('BeltRankUtil', () => {

  describe('getCurrentBeltIndex', () => {
    it('should return 0 for undefined rank', () => {
      expect(BeltRankUtil.getCurrentBeltIndex(undefined)).toBe(0);
    });

    it('should return 0 for empty string', () => {
      expect(BeltRankUtil.getCurrentBeltIndex('')).toBe(0);
    });

    it('should return correct index for each belt', () => {
      expect(BeltRankUtil.getCurrentBeltIndex('White')).toBe(0);
      expect(BeltRankUtil.getCurrentBeltIndex('Yellow')).toBe(1);
      expect(BeltRankUtil.getCurrentBeltIndex('Green')).toBe(2);
      expect(BeltRankUtil.getCurrentBeltIndex('Blue')).toBe(3);
      expect(BeltRankUtil.getCurrentBeltIndex('Brown')).toBe(4);
      expect(BeltRankUtil.getCurrentBeltIndex('Black')).toBe(5);
    });

    it('should be case insensitive', () => {
      expect(BeltRankUtil.getCurrentBeltIndex('white')).toBe(0);
      expect(BeltRankUtil.getCurrentBeltIndex('BLACK')).toBe(5);
      expect(BeltRankUtil.getCurrentBeltIndex('Blue Belt')).toBe(3);
    });

    it('should return -1 for unknown rank', () => {
      expect(BeltRankUtil.getCurrentBeltIndex('Purple')).toBe(-1);
    });
  });

  describe('getBeltProgression', () => {
    it('should return all 6 belts', () => {
      expect(BeltRankUtil.getBeltProgression('White').length).toBe(6);
    });

    it('should mark only White as achieved for White rank', () => {
      const progression = BeltRankUtil.getBeltProgression('White');
      expect(progression[0]).toEqual({ belt: 'White', achieved: true });
      expect(progression[1]).toEqual({ belt: 'Yellow', achieved: false });
      expect(progression[2]).toEqual({ belt: 'Green', achieved: false });
    });

    it('should mark all belts up to Blue as achieved', () => {
      const progression = BeltRankUtil.getBeltProgression('Blue');
      const achieved = progression.filter(p => p.achieved).map(p => p.belt);
      const notAchieved = progression.filter(p => !p.achieved).map(p => p.belt);
      expect(achieved).toEqual(['White', 'Yellow', 'Green', 'Blue']);
      expect(notAchieved).toEqual(['Brown', 'Black']);
    });

    it('should mark all belts as achieved for Black rank', () => {
      const progression = BeltRankUtil.getBeltProgression('Black');
      expect(progression.every(p => p.achieved)).toBeTrue();
    });

    it('should mark no belts as achieved for unknown rank', () => {
      const progression = BeltRankUtil.getBeltProgression('Purple');
      expect(progression.every(p => !p.achieved)).toBeTrue();
    });

    it('should be case insensitive', () => {
      const progression = BeltRankUtil.getBeltProgression('green belt');
      const achieved = progression.filter(p => p.achieved).map(p => p.belt);
      expect(achieved).toEqual(['White', 'Yellow', 'Green']);
    });
  });
});
