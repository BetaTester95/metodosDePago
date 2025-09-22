import { Routes } from '@angular/router';
import { UsuarioTabla } from './views/componentes/usuario-tabla/usuario-tabla';
import { WalletCreator } from './views/componentes/wallet-creator/wallet-creator';
import { AddCard } from './views/componentes/add-card/add-card';
import { Component } from '@angular/core';

export const routes: Routes = [
    {path: 'panelUsuarios', component: UsuarioTabla},
    {path: 'crearBilleteraVirtual', component: WalletCreator},
    {path: 'addCard', component: AddCard}
];
