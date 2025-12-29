export interface MonthlyTestActivity {
  year: number;
  month: number;
  testCount: number;
}

export interface RankDistribution {
  rank: string;
  count: number;
}

export interface ProgressionAnalytics {
  totalEnrollments: number;
  totalTests: number;
  passedTests: number;
  failedTests: number;
  passRate: number;
  totalPromotions: number;
  averageDaysToBlue: number;
  mostActiveTestingMonths: MonthlyTestActivity[];
  currentRankDistribution: RankDistribution[];
}

export interface StudentEvent {
  eventId: string;
  eventType: string;
  occurredAt: string;
  version: number;
  eventData: string;
}

export interface StudentRankAtDate {
  rank: string;
  enrolledDate: string | null;
  lastTestDate: string | null;
  lastTestNotes: string | null;
  totalEventsProcessed: number;
}
