import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LetterPopupComponent } from './letter-popup.component';

describe('LetterPopupComponent', () => {
  let component: LetterPopupComponent;
  let fixture: ComponentFixture<LetterPopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [LetterPopupComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LetterPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
