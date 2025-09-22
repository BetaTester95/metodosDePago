import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AgregarTarjetaService, TarjetaRequest } from 'src/app/servicios/agregar-tarjeta.servicios';
import { Validation } from 'src/app/utils/validation';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-add-card',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './add-card.html',
  styleUrl: './add-card.css'
})
export class AddCard {
  tiposBilletera = [
    { label: 'Mercado Pago', value: 'mercadopago' },
    { label: 'Cuenta DNI', value: 'cuentadni' },
    { label: 'PayPal', value: 'paypal' }
  ];
  tipoBilletera: 'mercadopago' | 'cuentadni' | 'paypal' | null = null;

  nombre = '';
  apellido = '';
  numTarjeta = '';
  dni!: number;
  fechaVenc = '';
  cod = '';
  mensajeError: string = '';

  constructor(private agregarTarjetaService: AgregarTarjetaService, private _validaciones: Validation) { }

  guardarTarjeta() {

    if (!this.tipoBilletera) return

    if (!this._validaciones.validarNombre(this.nombre)){
      this.mensajeError = 'Error al validar el nombre'
      return
    }

    if (!this._validaciones.validarApellido(this.apellido)){
      this.mensajeError = 'Erro al validar el apellido'
      return
    }

    if (!this._validaciones.validarDni(this.dni)){
      this.mensajeError = 'Error al validar el dni'
      return
    }

    if (!this._validaciones.validarFecha(this.fechaVenc)){
      this.mensajeError = 'Error al validar la fecha'
      return
    }


    const dataTarjeta = {
      numTarjeta: this.numTarjeta,
      nombre: this.nombre,
      apellido: this.apellido,
      dni: this.dni,
      fechaVenc: this.fechaVenc,
      cod: this.cod
    }

    this.agregarTarjetaService.agregarTarjeta(this.tipoBilletera, dataTarjeta).subscribe({
      next: respuesta => {

        this.creadoExitoso(respuesta.mensaje)

      },
      error: err => {
        console.error('Error al agregar la tarjeta', err);
        const mensajeError = err.error.mensaje
        console.log('mensaje de erro backend', mensajeError)
        if(err.error?.status == 400){
          this.mensajeError = 'Error en la validacion'
        }else{
          this.mensajeError = err.error?.mensaje
        }
      }
    })
  }

  creadoExitoso(titulo: string) {
    Swal.fire({
      title: titulo,
      icon: "success",
      draggable: true
    });
  }

}
