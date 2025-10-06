import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class Validation {

  // Nombre y apellido: solo letras, mínimo 2 caracteres
  validarNombre(nombre?: string): boolean {
    return /^[a-zA-Z]{2,}$/.test(nombre ?? '');
  }

  validarApellido(apellido?: string): boolean {
    return /^[a-zA-Z]{2,}$/.test(apellido ?? '');
  }

  // DNI: solo números, entre 7 y 8 dígitos
  validarDni(dni?: number): boolean {
    return /^\d{8}$/.test(dni?.toString() ?? '');
  }

  // Email: patrón estándar de correo
  validarEmail(email?: string): boolean {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email ?? '');
  }

  // Password mínimo 6 caracteres, al menos un número y una letra
  validarPassword(password?: string): boolean {
    return /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$/.test(password ?? '');
  }

  validarFecha(fecha: Date): boolean {
    if (!fecha) return false;

    const hoy = new Date();
    hoy.setHours(0, 0, 0, 0); // resetear horas para comparar solo la fecha

    const fechaIngresada = new Date(fecha);
    return fechaIngresada >= hoy;
  }

  validarNumeroTarjeta(numero: string): boolean {
    if (!numero) return false;
    const regex = /^[0-9]{16,22}$/;
    return regex.test(numero);
  }

}
