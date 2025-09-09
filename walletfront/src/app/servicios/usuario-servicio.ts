import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})

export class UsuarioServicio {
    private apiUrl = 'http://localhost:5055/api/usuarios';
    private apiUrlLogin = 'http://localhost:5055/api/auth/login';
    constructor(private http: HttpClient){
    }
    crearUsuario(usuarioData: any): Observable<any>{
        return this.http.post<any>(this.apiUrl, usuarioData) //usuario datas
    }

    loginUsuario(usuarioLogin: any): Observable<any>
    {
        return this.http.post<any>(this.apiUrlLogin, usuarioLogin) //usuario login
    }
}