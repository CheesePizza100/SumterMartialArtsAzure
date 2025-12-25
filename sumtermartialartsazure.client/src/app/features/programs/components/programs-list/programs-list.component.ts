import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProgramsService } from '../../services/programs.service';
import { Program, ProgramCategory } from '../../models/program.model';
import { ProgramFilters } from '../../utils/program-filters.util';
import { ProgramFiltersComponent } from '../program-filters/program-filters.component';
import { ProgramCardComponent } from '../program-card/program-card.component';


@Component({
  selector: 'app-programs-list',
  standalone: true,
  imports: [CommonModule, ProgramFiltersComponent, ProgramCardComponent],
  templateUrl: './programs-list.component.html',
  styleUrls: ['./programs-list.component.css']
})
export class ProgramsListComponent implements OnInit {
  programs: Program[] = [];
  filteredPrograms: Program[] = [];
  currentFilter: ProgramCategory = 'all';

  constructor(private programsService: ProgramsService) { }

  ngOnInit(): void {
    this.loadPrograms();
  }

  private loadPrograms(): void {
    this.programsService.getPrograms().subscribe({
      next: (data) => {
        this.programs = data;
        this.filteredPrograms = data;
      },
      error: (err) => console.error('Error loading programs:', err)
    });
  }

  onFilterChange(filter: ProgramCategory): void {
    this.currentFilter = filter;
    this.filteredPrograms = ProgramFilters.filterByCategory(this.programs, filter);
  }
}
