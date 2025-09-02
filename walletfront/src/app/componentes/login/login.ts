import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UsuarioServicio } from 'src/app/servicios/usuario-servicio';

@Component({
  selector: 'app-login',
  imports: [CommonModule,FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {

  constructor(private UsuarioServicio: UsuarioServicio) { }
  email: string = "";
  password_hash: string = "";



  ejecutarLogin() {
    const loginUsuario = {
      email: this.email,
      password_hash: this.password_hash
    };

    this.UsuarioServicio.loginUsuario(loginUsuario).subscribe({
      next: respuesta => console.log('Usuario logueado', respuesta),
      error: err => console.log('Error', err)
    })
  }
}
