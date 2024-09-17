import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FlightSearchDummyComponent } from './flight-search-dummy.component';

describe('FlightSearchDummyComponent', () => {
  let component: FlightSearchDummyComponent;
  let fixture: ComponentFixture<FlightSearchDummyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [FlightSearchDummyComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(FlightSearchDummyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
