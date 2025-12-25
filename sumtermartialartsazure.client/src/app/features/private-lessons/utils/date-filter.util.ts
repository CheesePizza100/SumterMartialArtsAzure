export class DateFilterUtil {
  /**
   * Creates a date filter function for mat-datepicker
   * Only allows dates that exist in the available dates array
   */
  static createDateFilter(availableDates: Date[]): (d: Date | null) => boolean {
    return (d: Date | null): boolean => {
      if (!d) return false;
      return availableDates.some(
        availableDate => availableDate.toDateString() === d.toDateString()
      );
    };
  }

  /**
   * Creates a date class function for mat-datepicker
   * Adds CSS class to available dates
   */
  static createDateClass(availableDates: Date[]): (d: Date) => string {
    return (d: Date): string => {
      const isAvailable = availableDates.some(
        availableDate => availableDate.toDateString() === d.toDateString()
      );
      return isAvailable ? 'available-date' : '';
    };
  }
}
