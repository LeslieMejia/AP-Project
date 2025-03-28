import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployerLoginComponent } from './employer-login.component';

describe('EmployerLoginComponent', () => {
  let component: EmployerLoginComponent;
  let fixture: ComponentFixture<EmployerLoginComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EmployerLoginComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(EmployerLoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
