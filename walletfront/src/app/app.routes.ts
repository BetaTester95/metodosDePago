import { Routes } from '@angular/router';
import { UsuarioTabla } from './views/componentes/usuario-tabla/usuario-tabla';
import { WalletCreator } from './views/componentes/wallet-creator/wallet-creator';

export const routes: Routes = [
    {path: 'panelUsuarios', component: UsuarioTabla},
    {path: 'crearBilleteraVirtual', component: WalletCreator}
];
