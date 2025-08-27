import { IPagoStrategy } from '../estrategias/pago-estrategia.interface';

export class CtaDniEstrategia implements IPagoStrategy {

  pagoConTransferencia(montoPagar: number, cbu: string): boolean {
    console.log(`Pago con transferencia desde CTA DNI: $${montoPagar} al CBU ${cbu}`);
    return true;
  }

  agregarTarjeta(numTarjeta: string, nombre: string, apellido: string, dni: number, fechaVENC: Date, cod: number): boolean {
    console.log(`Agregando tarjeta CTA DNI: ${numTarjeta} de ${nombre} ${apellido}`);

    return true;
  }

  pagoConTarjetaCredito(montoPagar: number, cantCuotas: number): boolean {
    console.log(`Pago con tarjeta crédito CTA DNI: $${montoPagar} en ${cantCuotas} cuotas`);
    return true;
  }
}
