import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Observable } from 'rxjs';
declare var bootstrap: any;
import { UsuarioServicio } from 'src/app/servicios/usuario-servicio';
import { Validation } from 'src/app/utils/validation';

@Component({
  selector: 'app-usuario-tabla',
  imports: [CommonModule, FormsModule],
  templateUrl: './usuario-tabla.html',
  styleUrl: './usuario-tabla.css'
})
export class UsuarioTabla {
  usuarios: any[] = [];
  cargando = true;
  usuarioSeleccionado?: any;
  mostrarModalAgregar = false;
  mostrarModalEditar = false;
  //bool validaciones
  errorNombre: boolean = false;
  errorApellido: boolean = false;
  errorEmail: boolean = false;
  errorPass: boolean = false;
  errorDni: boolean = false;
  errorBackendDni: string = '';
  errorBackendEmail: string = '';

  //bool validaciones

  nuevoUsuario = {
    nombre: '',
    apellido: '',
    email: '',
    passwordHash: '',
    dni: 0,
    TipoUsuario: ''
  };

  usuarioEditando: any = {
    idUsuario: 0,
    nombre: '',
    apellido: '',
    email: '',
    dni: 0
  };


  constructor(private UsuarioServicio: UsuarioServicio, private validation: Validation) { }


  ngOnInit() {
    this.cargarUsuarios();
    this.cargando = false;
  }

  cargarUsuarios() {
    this.UsuarioServicio.listarUsuarios().subscribe(
      (data: any[]) => {
        this.usuarios = data;
      }
    )
  }



  altaUser() {

    if (!this.validation.validarNombre(this.nuevoUsuario.nombre)) {
      this.errorNombre = true;
      return
    }

    if (!this.validation.validarApellido(this.nuevoUsuario.apellido)) {
      this.errorApellido = true;
      return
    }

    if (!this.validation.validarEmail(this.nuevoUsuario.email)) {
      this.errorEmail = true;
      return
    }

    if (!this.validation.validarDni(this.nuevoUsuario.dni)) {
      this.errorDni = true;
      return
    }

    const users = {
      Nombre: this.nuevoUsuario.nombre,
      Apellido: this.nuevoUsuario.apellido,
      Email: this.nuevoUsuario.email,
      PasswordHash: this.nuevoUsuario.passwordHash,
      Dni: this.nuevoUsuario.dni,
      IdTipoUsuario: this.nuevoUsuario.TipoUsuario
    };

    this.UsuarioServicio.createUser(users).subscribe({
      next: (respuesta) => {
        console.log('Usuario Creado:', respuesta)
        if (respuesta.mensajeDni) {
          this.errorBackendDni = respuesta.mensajeDni;
        }
        if (respuesta.mensajeEmail) {
          this.errorBackendEmail = respuesta.mensajeEmail;
        }

        // Si no hay errores, limpiar el formulario
        if (!respuesta.mensajeDni && !respuesta.mensajeEmail) {
          this.limpiarModal();
        }
        this.nuevoUsuario = {
          nombre: '',
          apellido: '',
          email: '',
          passwordHash: '',
          dni: 0,
          TipoUsuario: ''
        };
        this.limpiarErrores();
      },
      error: (err) => {
        console.error('Error al crear usuario:', err);

      }
    })
  }

  abrirModalEliminar(usuario: any) {
    this.usuarioSeleccionado = usuario
    const modalElement = document.getElementById('confirmDeleteModal')
    const modal = new bootstrap.Modal(modalElement!)
    modal.show();
  }

  abrirModalEditar(usuario: any) {
    console.log(usuario)
    this.usuarioEditando = usuario
    this.mostrarModalEditar = true;
  }

  cerrarModalEditar() {
    this.mostrarModalEditar = false;
  }


  guardarCambios() {
    const users = {
      IdUsuario: this.usuarioEditando.idUsuario,
      Nombre: this.usuarioEditando.nombre,
      Apellido: this.usuarioEditando.apellido,
      Email: this.usuarioEditando.email,
      Dni: this.usuarioEditando.dni
    };

    this.UsuarioServicio.ediUser(users).subscribe({
      next: (respuesta) => {
        console.log('Usuario actualizado', respuesta);
        alert('Usuario actualizado con éxito');
        this.cerrarModalEditar();

        // refrescar lista si hace falta
        this.cargarUsuarios();
      },
      error: (err) => {
        console.log(users)
        console.error('Error al actualizar usuario:', err);
        alert('No se pudo actualizar el usuario.');
      }
    });
  }

  confirmarEliminar() {
    if (!this.usuarioSeleccionado) return;

    this.UsuarioServicio.deleteUser(this.usuarioSeleccionado.idUsuario).subscribe({
      next: () => {
        this.cargarUsuarios();  // recargamos la tabla
        this.usuarioSeleccionado = undefined;

        // Cerramos modal
        const modalElement = document.getElementById('confirmDeleteModal');
        const modal = bootstrap.Modal.getInstance(modalElement!);
        modal?.hide();
      },
      error: (err) => console.error('Error al eliminar', err)
    });
  }

  abrirModalAgregar() {
    this.usuarioSeleccionado = {};
    this.mostrarModalAgregar = true;
    this.limpiarErrores();
    this.limpiarModal();
  }

  cerrarModalAgregar() {
    this.mostrarModalAgregar = false;
  }

  limpiarErrores() {
    this.errorNombre = false;
    this.errorApellido = false;
    this.errorEmail = false;
    this.errorDni = false;
    this.errorBackendEmail = '';
    this.errorBackendDni = '';
  }


  limpiarModal() {
    // Limpiar los campos del formulario
    this.nuevoUsuario = {
      nombre: '',
      apellido: '',
      email: '',
      passwordHash: '',
      dni: 0,
      TipoUsuario: ''
    };
  }
}
