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
  email: string = '';
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


  // Métodos para manejar el inputs dinamicos
  getLabelText(): string {
    return this.tipoBilletera === 'paypal' ? 'Email' : 'DNI';
  }

  getInputType(): string {
    return this.tipoBilletera === 'paypal' ? 'email' : 'number';
  }

  getPlaceholderText(): string {
    return this.tipoBilletera === 'paypal' ? 'Ingrese email' : 'Ingrese DNI';
  }

  getInputValue(): string | number | null {
    return this.tipoBilletera === 'paypal' ? this.email : this.dni;
  }

  updateInputValue(value: any): void {
    if (this.tipoBilletera === 'paypal') {
      this.email = value;
    } else {
      this.dni = value;
    }
  }

  onTipoBilleteraChange(newValue: any): void {
    console.log('Cambio de tipo:', newValue);
    this.tipoBilletera = newValue;
  }

  crearWallet(): void {
    console.log('tipoBilletera:', this.tipoBilletera);
    console.log('dni:', this.dni);
    console.log('email:', this.email);
    console.log('typeof tipoBilletera:', typeof this.tipoBilletera);
    this.mensajeError = '';

    if (!this.tipoBilletera) {
      this.mensajeError = 'seleccionar un tipo de billetera para crear'
      return
    }

    if (this.tipoBilletera == 'paypal') {
      let debugg = 'paypal'
      console.log(debugg)
      if (!this._validation.validarEmail(this.email)) {
        this.mensajeError = 'Error al validar el email'
        return
      }
    } else {

      if (!this.dni) {
        this.mensajeError = 'Error al validar el dni'
        return
      }

      if (!this._validation.validarDni(this.dni)) {
        this.mensajeError = "El dni debe ser mayor a 8 digitos."
        return
      }
    }

    const identificador = this.tipoBilletera == 'paypal' ? this.email : this.dni
    if (!identificador) {
      this.mensajeError = this.tipoBilletera === 'paypal' ? 'El email es requerido' : 'El DNI es requerido';
      return;
    }

    this.CrearBileterasServicios.crearBilletera(this.tipoBilletera, identificador).subscribe({
      next: (respuesta) => {
        console.log('next:', respuesta)
        if(respuesta.success === true){
          console.log('next ok: ', respuesta)
          this.creadoExitoso()          
        }else{
          console.log('Error back:', respuesta)
          this.mensajeError = respuesta.message
          
        }
      },
      error: (err) => {
        console.log('Error backend:', err);
        console.log('Mensaje: ', err.error?.message)
      }
    })
  }

  creadoExitoso() {
    Swal.fire({
        title: "Billetera creada exitosamente!",
        icon: "success",
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
