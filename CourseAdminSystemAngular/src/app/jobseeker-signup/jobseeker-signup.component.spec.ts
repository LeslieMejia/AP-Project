import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JobseekerSignupComponent } from './jobseeker-signup.component';

describe('JobseekerSignupComponent', () => {
  let component: JobseekerSignupComponent;
  let fixture: ComponentFixture<JobseekerSignupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [JobseekerSignupComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(JobseekerSignupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
