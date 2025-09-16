import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WalletCreator } from './wallet-creator';

describe('WalletCreator', () => {
  let component: WalletCreator;
  let fixture: ComponentFixture<WalletCreator>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WalletCreator]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WalletCreator);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
