import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CargasSaldo } from 'src/app/servicios/cargas-saldo';
import { Validation } from 'src/app/utils/validation';

@Component({
  selector: 'app-cargar-saldo',
  imports: [CommonModule,FormsModule],
  templateUrl: './cargar-saldo.html',
  styleUrl: './cargar-saldo.css'
})
export class CargarSaldo {

  dni: number = 0;
  monto: number = 0;

  dniMp: number | undefined;
  montoMp: number | undefined;

  dniCtaDni: number | undefined;
  montoCtadDni: number | undefined;

  dniPayPal: number | undefined;
  montoPayPal: number | undefined;

  //variables errroes
  dniError: string = '';
  montoError: string = '';

  public constructor(private cargasSaldo: CargasSaldo, private _validation: Validation) { }


  guardarSaldoMp() {
    this.cargasSaldo.cargarMercadoPago(this.dni, this.monto).subscribe({

      next: respuesta =>{
        console.log("next:", respuesta);
      },
      error: err =>{
        console.log("error:", err);
      }
    })
  }

  cargarSaldoCtaDni(){
      this.cargasSaldo.cargarCuentaDni(this.dni, this.monto).subscribe({

      next: respuesta =>{
        console.log("next:", respuesta);
      },
      error: err =>{
        console.log("error:", err);
      }
    })
  }

  cargarSaldoPayPaL(){
      this.cargasSaldo.cargarSaldoPaypal(this.dni, this.monto).subscribe({

      next: respuesta =>{
        console.log("next:", respuesta);
      },
      error: err =>{
        console.log("error:", err);
      }
    })
  }
  


}//fin de la clase
