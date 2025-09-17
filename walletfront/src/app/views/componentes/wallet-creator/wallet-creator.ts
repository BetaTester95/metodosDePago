import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BileterasServicios } from 'src/app/servicios/crear-billeteras.servicios';
import { Validation } from 'src/app/utils/validation';
import Swal from 'sweetalert2';


@Component({
  selector: 'app-wallet-creator',
  imports: [CommonModule, FormsModule],
  templateUrl: './wallet-creator.html',
  styleUrl: './wallet-creator.css'
})
export class WalletCreator {
  dni: number | null = null;
  error: string = '';
  successMessage: string = '';
  mensaje: string = '';
  mensajeError: string = '';
  //msg 
  cvu: string = '';
  tipo: string = '';
  saldo?: number;
  nombre?: string = '';

  //

  tiposBilletera = [
    { label: 'Mercado Pago', value: 'mercadopago' },
    { label: 'Cuenta DNI', value: 'cuentadni' },
    { label: 'PayPal', value: 'paypal' }
  ];
  tipoBilletera: 'mercadopago' | 'cuentadni' | 'paypal' | null = null;

  
  constructor(private CrearBileterasServicios: BileterasServicios, private _validation: Validation) {
  }

  crearWallet(): void {
    if (!this.dni || !this.tipoBilletera) {
      this.mensajeError = 'Completa el DNI y selecciona un tipo de billetera'
      return
    }

    if (!this._validation.validarDni(this.dni)) {
      this.mensajeError = "El dni debe ser mayor a 8 digitos."
      return
    }

    this.CrearBileterasServicios.crearBilletera(this.tipoBilletera, this.dni).subscribe({
      next: (respuesta) => {
        console.log(respuesta);

        this.mensaje = respuesta.datos.mensaje
        this.nombre = respuesta.datos.nombre
        this.tipo = respuesta.datos.tipo;
        this.cvu = respuesta.datos.cvu;
        this.saldo = respuesta.datos.saldo;
        this.creadoExitoso(respuesta.mensaje, `\nCVU: ${this.cvu}\nSaldo: ${this.saldo}`, 'success')
      },
      error: (err) => {
        console.log('Error backend:', err);
        console.log('Mensaje: ', err.error?.mensaje)
        console.log(this.mensaje)
        if (err.error?.status == 400) {
          this.mensajeError = 'Error en la validación'
        } else {
          this.mensajeError = err.error?.mensaje
        }
      }
    })
  }

  creadoExitoso(titulo: string, mensaje: string, tipo: 'success'): void {
    Swal.fire({
      title: titulo,
      text: mensaje,
      icon: tipo,
      draggable: true
    });
  }

  /*errorCrear(titulo: string, mensaje: string, tipo: 'error'): void {
    Swal.fire({
      icon: tipo,
      title: titulo,
      text: mensaje    
    });
  }*/

}
