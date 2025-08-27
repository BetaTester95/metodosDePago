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
  mail: string = "";
  tipoBilletera: string = "";
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
     if (this.mail == "" || this.mail == undefined) {
       this.errorMail = true;
    }
    const usuario = {
      nombre: this.nombre,
      apellido: this.apellido,
      dni: this.dni
    };
    this.UsuarioServicio.crearUsuario(usuario).subscribe({
      next: respuesta => console.log('Usuario creado', respuesta),
      error: err => console.log('Error',err)
    });

    return this.erroSalida; 
  }
}
