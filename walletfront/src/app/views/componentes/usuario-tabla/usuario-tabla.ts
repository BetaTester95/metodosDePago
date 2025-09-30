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
  errorGeneral: string = '';
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
        console.log("error listar usuarios: ", datos)
        this.usuarios = datos
      },
      error: (e) => {
                console.log("error listar usuarios: ", e)
        console.log(e)
      }
    });
  }

  altaUser() {

    if (!this.validation.validarNombre(this.nuevoUsuario.nombre) || !this.nuevoUsuario.nombre) {
      this.errorNombre = true;
      return
    }

    if (!this.validation.validarApellido(this.nuevoUsuario.apellido) || !this.nuevoUsuario.apellido) {
      this.errorApellido = true;
      return
    }

    if (!this.validation.validarEmail(this.nuevoUsuario.email) || !this.nuevoUsuario.email) {
      this.errorEmail = true;
      return
    }

    if (!this.validation.validarDni(this.nuevoUsuario.dni) || !this.nuevoUsuario.dni) {
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
        console.log(respuesta)
        if(respuesta.isSuccess === true){
        console.log('next: ', respuesta)
        this.cargarUsuarios();
        this.alertSuces("Usuario creado correctamente. ");
        this.limpiarModal();
        }else{
          if(respuesta.mensaje.includes('dni')){
            this.errorBackendDni = respuesta.mensaje
          }else{
            this.errorBackendEmail = respuesta.mensaje
          }
        }
      },
      error: (err) => {
        console.error('Ocurrio un error: ', err)
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
    this.limpiarErrores();
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
        console.log('next antes de llegar al if', respuesta)
      if (respuesta.isSuccess === true) {
          console.log('Usuario actualizado:', respuesta);
          this.alertSuces("Usuario editado correctamente. ");
          this.cerrarModalEditar();
          this.cargarUsuarios();
          this.limpiarErrores();
        }else{
          console.log('Error al actualizar: ', respuesta)
          if(respuesta.message.includes('dni')){
            this.errorBackendDni = respuesta.message
          }else{
            this.errorBackendEmail = respuesta.message
          }
        }        
      },
      error: (err) => {
        console.error('Ocurrio un error: ', err)       
        console.log('eror al actualiar', err)
      }
    });
  }

  confirmarEliminar() {
    if (!this.usuarioSeleccionado) return;

    this.UsuarioServicio.deleteUser(this.usuarioSeleccionado.idUsuario).subscribe({
      next: () => {
        this.cargarUsuarios();
        this.usuarioSeleccionado = undefined;
        this.alertSuces("Usuario eliminado correctamente. ")
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

  alertSuces(titulo: string) { //alerta ok
    Swal.fire({
      position: "top-end",
      icon: "success",
      title: titulo,
      showConfirmButton: false,
      timer: 1600
    });
  }
}
