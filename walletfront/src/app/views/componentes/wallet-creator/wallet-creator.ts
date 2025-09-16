import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BileterasServicios } from 'src/app/servicios/crear-billeteras.servicios';
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
  //msg 
  cvu: string = '';
  tipo: string = '';
  saldo?: number;

  //

  tiposBilletera = [
    { label: 'Mercado Pago', value: 'mercadopago' },
    { label: 'Cuenta DNI', value: 'cuentadni' },
    { label: 'PayPal', value: 'paypal' }
  ];
  tipoBilletera: 'mercadopago' | 'cuentadni' | 'paypal' | null = null;



  constructor(private CrearBileterasServicios: BileterasServicios) {
  }

 crearWallet() {
  if (!this.dni || !this.tipoBilletera) {
    this.mensaje = 'Completa el DNI y selecciona un tipo de billetera';
    return;
  }

  this.CrearBileterasServicios.crearBilletera(this.tipoBilletera, this.dni).subscribe({
    next: (respuesta) => {
      console.log(respuesta);

      this.tipo = respuesta.datos.tipo;
      this.cvu = respuesta.datos.cvu;
      this.saldo = respuesta.datos.saldo;

    },
    error: (err) => {
      console.error(err);
      this.mensaje = 'Error al crear la billetera';
    }
  });
}



  creadoExitoso() {
    Swal.fire({
      title: "Drag me!",
      icon: "success",
      draggable: true
    });
  }




}
