import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { HttpParams } from '@angular/common/http';

export interface TarjetaRequest {
  numTarjeta: string;
  nombre: string;
  apellido: string;
  dni: number;
  fechaVenc: string; // formato ISO
  cod: string;
}


@Injectable({
    providedIn: 'root'
})

export class AgregarTarjetaService{

    
    
    private addCardUrl = "http://localhost:5055/api/estrategias/agregar-tarjeta";

    constructor(private http: HttpClient){}

    agregarTarjeta(tipoMetodoPago: string, body: TarjetaRequest): Observable<any>{
        const params = new HttpParams().set('tipoMetodoPago', tipoMetodoPago)
        return this.http.post(this.addCardUrl, body, {params})
    }
    

}