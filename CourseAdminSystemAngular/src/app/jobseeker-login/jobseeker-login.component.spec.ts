import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JobseekerLoginComponent } from './jobseeker-login.component';

describe('JobseekerLoginComponent', () => {
  let component: JobseekerLoginComponent;
  let fixture: ComponentFixture<JobseekerLoginComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [JobseekerLoginComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(JobseekerLoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
