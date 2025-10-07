import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CargarSaldo } from './cargar-saldo';

describe('CargarSaldo', () => {
  let component: CargarSaldo;
  let fixture: ComponentFixture<CargarSaldo>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CargarSaldo]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CargarSaldo);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
