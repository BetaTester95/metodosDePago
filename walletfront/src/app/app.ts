import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Formulario } from '../app/componentes/formulario/formulario';
import { Login } from './componentes/login/login';
import { Dashboard } from './componentes/dashboard/dashboard';
import { UsuarioTabla } from './views/componentes/usuario-tabla/usuario-tabla';
import { WalletCreator } from './views/componentes/wallet-creator/wallet-creator';

@Component({
  selector: 'app-root',
  imports: [UsuarioTabla, WalletCreator],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('walletfront');
}
