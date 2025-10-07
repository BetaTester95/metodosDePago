import { Routes } from '@angular/router';
import { UsuarioTabla } from './views/componentes/usuario-tabla/usuario-tabla';
import { WalletCreator } from './views/componentes/wallet-creator/wallet-creator';
import { AddCard } from './views/componentes/add-card/add-card';
import { CargarSaldo } from './views/componentes/cargar-saldo/cargar-saldo';

export const routes: Routes = [
    {path: 'panelUsuarios', component: UsuarioTabla},
    {path: 'crearBilleteraVirtual', component: WalletCreator},
    {path: 'addCard', component: AddCard},
    {path: 'cargarSaldo', component: CargarSaldo}
];
