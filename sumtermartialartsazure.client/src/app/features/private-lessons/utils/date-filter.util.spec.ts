import { DateFilterUtil } from './date-filter.util';

describe('DateFilterUtil', () => {
  const availableDates = [
    new Date(2026, 2, 15),
    new Date(2026, 2, 16),
    new Date(2026, 2, 20)
  ];

  describe('createDateFilter', () => {
    let filter: (d: Date | null) => boolean;

    beforeEach(() => {
      filter = DateFilterUtil.createDateFilter(availableDates);
    });

    it('should return a function', () => {
      expect(filter).toBeInstanceOf(Function);
    });

    it('should return false for null', () => {
      expect(filter(null)).toBeFalse();
    });

    it('should return true for a date in the available list', () => {
      expect(filter(new Date(2026, 2, 15))).toBeTrue();
      expect(filter(new Date(2026, 2, 16))).toBeTrue();
      expect(filter(new Date(2026, 2, 20))).toBeTrue();
    });

    it('should return false for a date not in the available list', () => {
      expect(filter(new Date(2026, 2, 17))).toBeFalse();
      expect(filter(new Date(2026, 3, 1))).toBeFalse();
    });

    it('should return false when available dates array is empty', () => {
      const emptyFilter = DateFilterUtil.createDateFilter([]);
      expect(emptyFilter(new Date(2026, 3, 15))).toBeFalse();
    });

    it('should match by date string ignoring time', () => {
      const dateWithTime = new Date(2026, 2, 15, 12, 0, 0);
      expect(filter(dateWithTime)).toBeTrue();
    });

    describe('createDateClass', () => {
      let dateClass: (d: Date) => string;

      beforeEach(() => {
        dateClass = DateFilterUtil.createDateClass(availableDates);
      });

      it('should return a function', () => {
        expect(dateClass).toBeInstanceOf(Function);
      });

      it('should return available-date for a date in the list', () => {
        expect(dateClass(new Date(2026, 2, 15))).toBe('available-date');
        expect(dateClass(new Date(2026, 2, 20))).toBe('available-date');
      });

      it('should return empty string for a date not in the list', () => {
        expect(dateClass(new Date(2026, 2, 17))).toBe('');
        expect(dateClass(new Date(2026, 3, 1))).toBe('');
      });

      it('should return empty string when available dates array is empty', () => {
        const emptyDateClass = DateFilterUtil.createDateClass([]);
        expect(emptyDateClass(new Date(2026, 2, 15))).toBe('');
      });

      it('should match by date string ignoring time', () => {
        const dateWithTime = new Date(2026, 2, 16, 12, 0, 0);
        expect(dateClass(dateWithTime)).toBe('available-date');
      });
    });
  });
});
