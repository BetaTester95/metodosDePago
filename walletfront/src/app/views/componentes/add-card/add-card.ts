import { CommonModule, DatePipe } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { from } from 'rxjs';
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
  numeroTarjeta = '';
  dni!: number;
  fechaVenc!: Date;
  cod = '';
  mensajeError: string = '';


  /*---------------------------------*/
  errorNombre: string = ''
  errorApellido: string = ''
  errorNumTarjeta: string = ''
  errorDni: string = '';
  errorFecha: string = '';


  constructor(private agregarTarjetaService: AgregarTarjetaService, private _validaciones: Validation, private datePipe: DatePipe) { }

  guardarTarjeta() {
  console.log('1️⃣ Fecha seleccionada en el input:', this.fechaVenc);

    if (!this.tipoBilletera) return

    if (!this._validaciones.validarNombre(this.nombre)){
      this.errorNombre = 'Error al validar el nombre'
      return
    }

    if (!this._validaciones.validarApellido(this.apellido)){
      this.errorApellido = 'Erro al validar el apellido'
      return
    }

    if (!this._validaciones.validarDni(this.dni)){
      this.errorDni = 'Error al validar el dni'
      return
    }

    if (!this._validaciones.validarFecha(this.fechaVenc)){
      this.errorFecha = 'Error al validar la fecha'
      return
    }



    console.log("antes de entrrar al objeto:", this.fechaVenc)
    const dataTarjeta = {
      numeroTarjeta: this.numeroTarjeta,
      nombre: this.nombre,
      apellido: this.apellido,
      dni: this.dni,
      fechaVenc: this.fechaVenc.toISOString(),
      cod: this.cod
    }

    

    this.agregarTarjetaService.agregarTarjeta(this.tipoBilletera, dataTarjeta).subscribe({
      next: respuesta => {
        console.log('next:', respuesta)
        if(respuesta.success === true){
          this.creadoExitoso("Tarjeta agregada correctamente. ");
        }else{
          console.log(respuesta);
          this.creadoError(respuesta.message);
        }
      },
      error: err => {
        console.error('Hubo error intentar mas tarde:', err);
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

  creadoError(titulo: string){
    Swal.fire({
    position: "top-end",
    icon: "error",
    title: titulo,
    showConfirmButton: false,
    timer: 3000
  });
  }

}
