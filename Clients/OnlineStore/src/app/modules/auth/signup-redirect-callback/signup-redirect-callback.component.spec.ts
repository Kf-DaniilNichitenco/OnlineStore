import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SignupRedirectCallbackComponent } from './signup-redirect-callback.component';

describe('SignupRedirectCallbackComponent', () => {
  let component: SignupRedirectCallbackComponent;
  let fixture: ComponentFixture<SignupRedirectCallbackComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SignupRedirectCallbackComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SignupRedirectCallbackComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
