import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UsuarioServicio } from 'src/app/servicios/usuario-servicio';
@Component({
  selector: 'app-formulario',
  /*  standalone: true,*/
  imports: [CommonModule, FormsModule],
  templateUrl: './formulario.html',
  styleUrl: './formulario.css'
})
export class Formulario {

  constructor(private UsuarioServicio: UsuarioServicio){}

  nombre?: string = "";
  apellido: string = "";
  dni?: number;
  email: string = "";
  password: string="";
  confirmPassword: string="";
  tipo_usuario: string = "";
  errorNombre: boolean = false;
  errorApellido: boolean = false;
  errorDni: boolean = false;
  errorMail: boolean = false;
  erroSalida:boolean = false;

  enviar() {
    if (this.nombre == "" || this.nombre == undefined) {
       this.errorNombre = true;
    }
     if (this.apellido == "" || this.apellido == undefined) {
       this.errorApellido = true;
    }
     if (this.dni == undefined || this.dni == null) {
       this.errorDni = true;
    }
     if (this.email == "" || this.email == undefined) {
       this.errorMail = true;
    }

    const usuario = {
      nombre: this.nombre,
      apellido: this.apellido,
      email: this.email,
      password_hash: this.password,
      dni: this.dni,
      tipo_usuario: this.tipo_usuario
    };
    // this.UsuarioServicio.crearUsuario(usuario).subscribe({
    //   next: respuesta => console.log('Usuario creado', respuesta),
    //   error: err => console.log('Error',err)
    // });

    return this.erroSalida; 
  }
}
