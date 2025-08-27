// pago-context.service.ts
import { Injectable } from '@angular/core';
import { IPagoStrategy } from '../estrategias/pago-estrategia.interface';

@Injectable({
  providedIn: 'root'
})
export class PagoContextService {
  private strategy!: IPagoStrategy;

  setStrategy(strategy: IPagoStrategy) {
    this.strategy = strategy;
  }

  pagoConTransferencia(monto: number, cbu: string) {
    return this.strategy.pagoConTransferencia(monto, cbu);
  }

  agregarTarjeta(num: string, nombre: string, apellido: string, dni: number, fechaVenc: Date, cod: number) {
    return this.strategy.agregarTarjeta(num, nombre, apellido, dni, fechaVenc, cod);
  }

  pagoConTarjetaCredito(monto: number, cuotas: number) {
    return this.strategy.pagoConTarjetaCredito(monto, cuotas);
  }
}
