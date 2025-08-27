// mercadopago.strategy.ts
import { IPagoStrategy } from '../estrategias/pago-estrategia.interface';

export class MercadoPagoStrategy implements IPagoStrategy {
  pagoConTransferencia(montoPagar: number, cbu: string): boolean {
    console.log(`Pago con transferencia MercadoPago: $${montoPagar} al CBU ${cbu}`);
    return true;
  }

  agregarTarjeta(numTarjeta: string, nombre: string, apellido: string, dni: number, fechaVenc: Date, cod: number): boolean {
    console.log(`Agregando tarjeta en MercadoPago: ${numTarjeta}`);
    return true;
  }

  pagoConTarjetaCredito(montoPagar: number, cantCuotas: number): boolean {
    console.log(`Pago con MercadoPago tarjeta crédito: $${montoPagar} en ${cantCuotas} cuotas`);
    return true;
  }
}
