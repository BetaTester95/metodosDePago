export interface IPagoStrategy {

  pagoConTransferencia(montoPagar: number, cbu: string): boolean;
  agregarTarjeta(numTarjeta: string, nombre: string, apellido: string, dni: number, fechaVenc: Date, cod: number): boolean;
  pagoConTarjetaCredito(montoPagar: number, cantCuotas: number): boolean;

}
