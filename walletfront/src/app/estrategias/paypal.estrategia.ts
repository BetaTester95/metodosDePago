// paypal.strategy.ts
import { IPagoStrategy } from '../estrategias/pago-estrategia.interface';

export class PaypalStrategy implements IPagoStrategy {
  pagoConTransferencia(montoPagar: number, cbu: string): boolean {
    console.log(`PayPal no soporta transferencia directa.`);
    return false;
  }

  agregarTarjeta(numTarjeta: string, nombre: string, apellido: string, dni: number, fechaVenc: Date, cod: number): boolean {
    console.log(`Agregando tarjeta en PayPal: ${numTarjeta}`);
    return true;
  }

  pagoConTarjetaCredito(montoPagar: number, cantCuotas: number): boolean {
    console.log(`Pago con PayPal tarjeta crédito: $${montoPagar}`);
    return true;
  }
}
