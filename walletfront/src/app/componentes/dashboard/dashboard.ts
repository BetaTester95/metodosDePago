import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-dashboard',
  imports: [CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})


export class Dashboard {
sidebarVisible: boolean = true;  // Controla si el sidebar está visible
ventanaActual: string = 'lista'; // Ventana actualmente seleccionada

 private usuariosEjemplo = [
    { id: 1, nombre: 'Juan Pérez', email: 'juan@ejemplo.com', rol: 'admin', activo: true },
    { id: 2, nombre: 'María García', email: 'maria@ejemplo.com', rol: 'editor', activo: true },
    { id: 3, nombre: 'Carlos López', email: 'carlos@ejemplo.com', rol: 'usuario', activo: false },
    { id: 4, nombre: 'Ana Martínez', email: 'ana@ejemplo.com', rol: 'editor', activo: true },
    { id: 5, nombre: 'Luis Rodríguez', email: 'luis@ejemplo.com', rol: 'usuario', activo: true }
  ];

  alternarSidebar(): void {
    this.sidebarVisible = !this.sidebarVisible;
  }

  /**
   * Cambia la ventana actual del dashboard
   * @param nuevaVentana - Nombre de la ventana a mostrar
   */
  cambiarVentana(nuevaVentana: string): void {
    this.ventanaActual = nuevaVentana;
  }

  /**
   * Obtiene las clases CSS para el sidebar según su estado
   * @returns String con las clases CSS del sidebar
   */
  obtenerClaseSidebar(): string {
    return this.sidebarVisible 
      ? 'col-md-3 col-lg-2' 
      : 'col-md-3 col-lg-2 sidebar-oculto';
  }

  /**
   * Obtiene las clases CSS para el contenido principal
   * @returns String con las clases CSS del contenido principal
   */
  obtenerClaseContenidoPrincipal(): string {
    return this.sidebarVisible 
      ? 'col-md-9 col-lg-10' 
      : 'col-md-12 contenido-expandido';
  }

  /**
   * Obtiene las clases CSS para los botones del menú
   * @param ventana - Nombre de la ventana del botón
   * @returns String con las clases CSS del botón
   */
  obtenerClaseBotonMenu(ventana: string): string {
    const claseBase = 'btn-menu w-100 text-start border-0';
    return this.ventanaActual === ventana 
      ? `${claseBase} active` 
      : claseBase;
  }

  /**
   * Obtiene el título de la ventana actual
   * @returns String con el título de la ventana
   */
  obtenerTituloVentanaActual(): string {
    const titulos: { [key: string]: string } = {
      'lista': 'Gestión de Usuarios',
      'crear': 'Crear Nuevo Usuario',
      'reportes': 'Reportes y Estadísticas',
      'configuracion': 'Configuración del Sistema'
    };
    return titulos[this.ventanaActual] || 'Dashboard';
  }

  /**
   * Obtiene la fecha actual formateada
   * @returns String con la fecha actual
   */
  obtenerFechaActual(): string {
    return new Date().toLocaleDateString('es-ES', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  }

  /**
   * Obtiene la lista de usuarios
   * @returns Array con los usuarios
   */
  obtenerListaUsuarios() {
    return this.usuariosEjemplo;
  }

  /**
   * Obtiene la clase CSS para el badge del rol
   * @param rol - Rol del usuario
   * @returns String con la clase CSS del badge
   */
  obtenerClaseRol(rol: string): string {
    const clases: { [key: string]: string } = {
      'admin': 'bg-danger',
      'editor': 'bg-warning',
      'usuario': 'bg-primary'
    };
    return clases[rol] || 'bg-secondary';
  }

  /**
   * Obtiene la clase CSS para el badge del estado
   * @param activo - Estado del usuario
   * @returns String con la clase CSS del badge
   */
  obtenerClaseEstado(activo: boolean): string {
    return activo ? 'bg-success' : 'bg-secondary';
  }

  /**
   * Cuenta los usuarios activos
   * @returns Número de usuarios activos
   */
  contarUsuariosActivos(): number {
    return this.usuariosEjemplo.filter(u => u.activo).length;
  }

  /**
   * Cuenta los usuarios inactivos
   * @returns Número de usuarios inactivos
   */
  contarUsuariosInactivos(): number {
    return this.usuariosEjemplo.filter(u => !u.activo).length;
  }

  /**
   * Cuenta los administradores
   * @returns Número de administradores
   */
  contarAdministradores(): number {
    return this.usuariosEjemplo.filter(u => u.rol === 'admin').length;


  }
}