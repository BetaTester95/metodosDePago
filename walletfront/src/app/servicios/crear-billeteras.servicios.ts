import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HttpParams } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})


export class BileterasServicios {

  private walletUrls = 
  {
    mercadopago: 'http://localhost:5055/api/billetera/crear/mercadopago',
    cuentadni: 'http://localhost:5055/api/billetera/crear/cuentadni',
    paypal: 'http://localhost:5055/api/billetera/crear/paypal'
  }

    constructor(private http: HttpClient){}

    crearBilletera(tipo: 'mercadopago' | 'cuentadni' | 'paypal', dni: number): Observable<any> {
    const url = this.walletUrls[tipo];
    const params = new HttpParams().set('dni', dni.toString());
    return this.http.post(url, null, { params });
  }
}
