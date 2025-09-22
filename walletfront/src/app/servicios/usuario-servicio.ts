import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { of } from 'rxjs';

@Injectable({
    providedIn: 'root'
})

export class UsuarioServicio {
    private apiUrl = 'http://localhost:5055/api/usuarios';
    private apiUrlLogin = 'http://localhost:5055/api/auth/login';
    private mostrarUsuarios = 'http://localhost:5055/api/Abm/mostrar/usuarios';
    private eliminarUserUrl = 'http://localhost:5055/api/Abm/eliminar/usuario';
    private crearUsuarioUrl = 'http://localhost:5055/api/Abm/crear/usuario';
    private editarUsuarioUrl = 'http://localhost:5055/api/Abm/editar/usuario';

    constructor(private http: HttpClient){
    }
    

    listarUsuarios(): Observable<any>
    {
        return this.http.get<any>(this.mostrarUsuarios)
    }

    createUser(usuario: any): Observable<any>
    {
        return this.http.post<any>(this.crearUsuarioUrl, usuario)
    }

    ediUser(usuario: any): Observable<any>
    {
        return this.http.put<any>(`${this.editarUsuarioUrl}/${usuario.idUsuario}`, usuario)
        .pipe(
            catchError((error) =>{
                 console.log('Error controlado, no aparece en consola automáticamente');
                // Retornamos un valor por defecto para que la suscripción no falle
                return of({ error: error.error?.error, mensaje: error.error?.mensaje });
            })
        )
    }

    // crearUsuario(usuarioData: any): Observable<any>{
    //     return this.http.post<any>(this.apiUrl, usuarioData) //usuario datas
    // }

    deleteUser(idUsuario: number){
        return this.http.delete<void>(`${this.eliminarUserUrl}/${idUsuario}`)
    }

     loginUsuario(usuarioLogin: any): Observable<any>
     {
         return this.http.post<any>(this.apiUrlLogin, usuarioLogin) //usuario login
     }
}