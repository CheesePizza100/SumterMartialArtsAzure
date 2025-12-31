import { BELT_RANKS, BeltRank } from '../models/instructor.model';

export class BeltRankUtil {
  static getCurrentBeltIndex(rank: string | undefined): number {
    if (!rank) return 0;
    const normalized = rank.toLowerCase();
    return BELT_RANKS.findIndex(belt => normalized.includes(belt.toLowerCase()));
  }

  static getBeltProgression(currentRank: string): { belt: BeltRank; achieved: boolean }[] {
    const currentIndex = this.getCurrentBeltIndex(currentRank);

    return BELT_RANKS.map((belt, index) => ({
      belt,
      achieved: index <= currentIndex
    }));
  }
}
