import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CargasSaldo {

  private apiUrlCargarSaldoMp = 'http://localhost:5055/api/billetera/cargar/mercadopago';
  private apiUrlCargarSaldoCtaDni = 'http://localhost:5055/api/billetera/cargar/ctadni';
  private apiUrlCargarSaldoPayPal = 'http://localhost:5055/api/billetera/cargr/paypal';

  constructor(private http: HttpClient) { }

  cargarMercadoPago(dni: number, monto: number): Observable<any> {
    const url = `${this.apiUrlCargarSaldoMp}?dni=${dni}&monto=${monto}`;
    return this.http.post(url, {});
  }

  cargarCuentaDni(dni: number, monto: number): Observable<any>{
    const url = `${this.apiUrlCargarSaldoCtaDni}?dni=${dni}&monto=${monto}`;
    return this.http.post(url, {})
  }

  cargarSaldoPaypal(dni: number, monto:number){
    const url = `${this.apiUrlCargarSaldoPayPal}?dni=${dni}&monto=${monto}`
    return this.http.post(url,{})
  }
}
