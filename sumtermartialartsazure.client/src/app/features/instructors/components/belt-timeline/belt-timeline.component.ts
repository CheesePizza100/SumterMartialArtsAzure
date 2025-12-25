import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BeltRankUtil } from '../../utils/belt-rank.util';
import { BELT_RANKS } from '../../models/instructor.model';

@Component({
  selector: 'app-belt-timeline',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './belt-timeline.component.html',
  styleUrls: ['./belt-timeline.component.css']
})
export class BeltTimelineComponent {
  @Input() set currentRank(rank: string) {
    this.progression = BeltRankUtil.getBeltProgression(rank);
  }

  progression: { belt: string; achieved: boolean }[] = [];
}
