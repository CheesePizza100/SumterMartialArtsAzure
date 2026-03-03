import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of, throwError } from 'rxjs';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';

import { ProgramDetailsComponent } from './program-details.component';
import { ProgramsService } from '../../services/programs.service';
import { Program } from '../../models/program.model';

const mockProgram: Program = {
  id: 1,
  name: 'Karate',
  description: 'Traditional karate',
  ageGroup: 'All Ages',
  details: 'Details here',
  duration: '1 hour',
  schedule: 'Mon/Wed 6pm',
  imageUrl: 'karate.jpg',
  instructors: []
};

describe('ProgramDetailsComponent', () => {
  let component: ProgramDetailsComponent;
  let fixture: ComponentFixture<ProgramDetailsComponent>;
  let programsServiceSpy: jasmine.SpyObj<ProgramsService>;
  let dialogSpy: jasmine.SpyObj<MatDialog>;

  const createComponent = async (paramId: string) => {
    await TestBed.configureTestingModule({
      imports: [ProgramDetailsComponent, NoopAnimationsModule],
      providers: [
        { provide: ProgramsService, useValue: programsServiceSpy },
        { provide: MatDialog, useValue: dialogSpy },
        {
          provide: ActivatedRoute,
          useValue: { snapshot: { paramMap: { get: () => paramId } } }
        }
      ]
    })
      .overrideComponent(ProgramDetailsComponent, {
        set: {
          imports: [CommonModule, RouterModule]
          // MatDialogModule omitted so dialogSpy wins
        }
      })
      .compileComponents();

    fixture = TestBed.createComponent(ProgramDetailsComponent);
    component = fixture.componentInstance;
  };

  beforeEach(() => {
    programsServiceSpy = jasmine.createSpyObj('ProgramsService', ['getProgramById']);
    dialogSpy = jasmine.createSpyObj('MatDialog', ['open']);
    programsServiceSpy.getProgramById.and.returnValue(of(mockProgram));
  });

  describe('loadProgram', () => {
    it('should create and load program on valid id', async () => {
      await createComponent('1');
      fixture.detectChanges();

      expect(component).toBeTruthy();
      expect(programsServiceSpy.getProgramById).toHaveBeenCalledWith(1);
      expect(component.program).toEqual(mockProgram);
      expect(component.isLoading).toBeFalse();
    });

    it('should set error when id is not a number', async () => {
      await createComponent('abc');
      fixture.detectChanges();

      expect(component.error).toBe('Invalid program ID');
      expect(component.isLoading).toBeFalse();
      expect(programsServiceSpy.getProgramById).not.toHaveBeenCalled();
    });

    it('should set error on service failure', async () => {
      spyOn(console, 'error');
      programsServiceSpy.getProgramById.and.returnValue(throwError(() => new Error('error')));
      await createComponent('1');
      fixture.detectChanges();

      expect(component.error).toBe('Failed to load program details');
      expect(component.isLoading).toBeFalse();
    });
  });

  describe('openEnrollDialog', () => {
    it('should do nothing if program is not loaded', async () => {
      await createComponent('1');
      component.program = undefined;
      component.openEnrollDialog();
      expect(dialogSpy.open).not.toHaveBeenCalled();
    });

    it('should open dialog with program data', async () => {
      await createComponent('1');
      fixture.detectChanges();

      component.openEnrollDialog();
      expect(dialogSpy.open).toHaveBeenCalledWith(
        jasmine.any(Function),
        { width: '400px', data: mockProgram }
      );
    });
  });
});
