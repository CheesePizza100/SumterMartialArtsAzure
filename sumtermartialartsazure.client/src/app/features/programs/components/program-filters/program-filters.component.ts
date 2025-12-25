import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

export type ProgramCategory = 'all' | 'kids' | 'adult' | 'competition';

@Component({
  selector: 'app-program-filters',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './program-filters.component.html',
  styleUrls: ['./program-filters.component.css']
})
export class ProgramFiltersComponent {
  @Input() currentFilter: ProgramCategory = 'all';
  @Output() filterChange = new EventEmitter<ProgramCategory>();

  filters = [
    { value: 'all' as ProgramCategory, label: 'All' },
    { value: 'kids' as ProgramCategory, label: 'Kids' },
    { value: 'adult' as ProgramCategory, label: 'Adult' },
    { value: 'competition' as ProgramCategory, label: 'Competition' }
  ];

  onFilterChange(filter: ProgramCategory): void {
    this.filterChange.emit(filter);
  }
}
