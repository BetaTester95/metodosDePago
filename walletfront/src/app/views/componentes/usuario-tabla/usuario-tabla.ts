import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Observable } from 'rxjs';
declare var bootstrap: any;
import { UsuarioServicio } from 'src/app/servicios/usuario-servicio';
import { Validation } from 'src/app/utils/validation';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-usuario-tabla',
  standalone: true,
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

  //bool validaciones inicio
  nuevoUsuario = {
    nombre: '',
    apellido: '',
    email: '',
    passwordHash: '',
    dni: undefined,
    TipoUsuario: ''
  };
  //bool validaciones fin

  usuarioEditando: any = {
    idUsuario: 0,
    nombre: '',
    apellido: '',
    email: '',
    dni: undefined
  };


  constructor(private UsuarioServicio: UsuarioServicio, private validation: Validation) { }


  ngOnInit() {
    this.cargarUsuarios();
    this.cargando = false;
  }

  cargarUsuarios() {
    this.UsuarioServicio.listarUsuarios().subscribe({
      next: (datos) => { 
        this.usuarios = datos
      },
      error: (e) => {
      console.log(e)
    }
    });
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
      console.log('SUCCESS: Usuario creado correctamente');
      this.cargarUsuarios();
      this.alertSuces();
      this.limpiarModal();
    },

    error: (err) => {
      console.log('Error del backend:', err);

      if (err.status === 409) {
        const mensaje = err.error?.mensaje || '';

        if (mensaje.toLowerCase().includes('email')) {
          this.errorBackendEmail = mensaje;
        } else if (mensaje.toLowerCase().includes('dni')) {
          this.errorBackendDni = mensaje;
        }
      } else {
        console.error('Error del servidor:', err);
      }
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
  this.limpiarErrores()
  this.cargarUsuarios()
}


guardarCambios() {

  if (!this.validation.validarNombre(this.usuarioEditando.nombre)) {
    this.errorNombre = true;
    return
  }

  if (!this.validation.validarApellido(this.usuarioEditando.apellido)) {
    this.errorApellido = true;
    return
  }

  if (!this.validation.validarEmail(this.usuarioEditando.email)) {
    this.errorEmail = true;
    return
  }

  if (!this.validation.validarDni(this.usuarioEditando.dni)) {
    this.errorDni = true;
    return
  }
  const users = {
    idUsuario: this.usuarioEditando.idUsuario,
    Nombre: this.usuarioEditando.nombre,
    Apellido: this.usuarioEditando.apellido,
    Email: this.usuarioEditando.email,
    Dni: this.usuarioEditando.dni
  };

  this.UsuarioServicio.ediUser(users).subscribe({
    next: (respuesta) => {

      // Todo ok
      console.log('Usuario actualizado:', respuesta);
      alert(respuesta.mensaje || 'Usuario actualizado con éxito');
      this.cerrarModalEditar();
      this.cargarUsuarios();
      this.limpiarErrores();

    },

    error: (err) => {
      console.error('Error al actualizar usuario:', err);
      alert('Error en el servidor, intente nuevamente.');
    }
  });
}

confirmarEliminar() {
  if (!this.usuarioSeleccionado) return;

  this.UsuarioServicio.deleteUser(this.usuarioSeleccionado.idUsuario).subscribe({
    next: () => {
      this.cargarUsuarios();
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
    dni: undefined,
    TipoUsuario: ''
  };
  this.limpiarErrores();
}

alertSuces() { //alerta ok
  Swal.fire({
    position: "top-end",
    icon: "success",
    title: "Usuario creado",
    showConfirmButton: false,
    timer: 1600
  });
}


}
