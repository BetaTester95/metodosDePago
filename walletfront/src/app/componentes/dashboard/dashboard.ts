import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';


interface WalletItem {
  id: string;
  name: string;
  icon: string;
  balance?: number;
  currency?: string;
}

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})


export class Dashboard {

  selectedWallet = 'dni';
  
  wallets: WalletItem[] = [
    { 
      id: 'dni', 
      name: 'Cuenta DNI', 
      icon: '🏛️', 
      balance: 15420.50, 
      currency: '$' 
    },
    { 
      id: 'paypal', 
      name: 'PayPal', 
      icon: '💳', 
      balance: 892.30, 
      currency: 'USD ' 
    },
    { 
      id: 'mercadopago', 
      name: 'Mercado Pago', 
      icon: '💰', 
      balance: 8750.75, 
      currency: '$' 
    },
    { 
      id: 'cobrador', 
      name: 'Cobrador', 
      icon: '📊', 
      balance: 25600.00, 
      currency: '$' 
    }
  ];

  // DNI Account data
  dniBalance = 15420.50;
  userDni = '12.345.678';
  lastMovementDate = 'Hoy 14:30';

  // PayPal data
  paypalBalance = 892.30;
  paypalEmail = 'usuario@email.com';

  // Mercado Pago data
  mercadoPagoBalance = 8750.75;
  mercadoPagoCVU = '0000003100010000000001';

  // Cobrador data
  cobradorBalance = 25600.00;
  monthlyCommissions = 1250.00;
  pendingCollections = 15;

  recentTransactions = [
    {
      icon: '🏪',
      description: 'Compra en Supermercado',
      date: 'Hoy 14:30',
      amount: -850.00
    },
    {
      icon: '💰',
      description: 'Transferencia recibida',
      date: 'Ayer 09:15',
      amount: 2500.00
    },
    {
      icon: '⚡',
      description: 'Pago de servicios',
      date: '2 días atrás',
      amount: -320.50
    },
    {
      icon: '📱',
      description: 'Recarga celular',
      date: '3 días atrás',
      amount: -100.00
    }
  ];

  selectWallet(walletId: string) {
    this.selectedWallet = walletId;
  }

  getCurrentWalletName(): string {
    const wallet = this.wallets.find(w => w.id === this.selectedWallet);
    return wallet ? wallet.name : 'Dashboard';
  }

  // Action methods
  addMoney() {
    alert('Funcionalidad: Agregar dinero');
  }

  sendMoney() {
    alert('Funcionalidad: Enviar dinero');
  }

  transferMoney() {
    alert('Funcionalidad: Transferir dinero');
  }

  payServices() {
    alert('Funcionalidad: Pagar servicios');
  }

  viewHistory() {
    alert('Funcionalidad: Ver historial');
  }

  withdrawPaypal() {
    alert('Funcionalidad: Retirar de PayPal');
  }

  sendPaypalMoney() {
    alert('Funcionalidad: Enviar dinero por PayPal');
  }

  requestMoney() {
    alert('Funcionalidad: Solicitar dinero');
  }

  payWithQR() {
    alert('Funcionalidad: Pagar con QR de Mercado Pago');
  }

  generateLink() {
    alert('Funcionalidad: Generar link de pago');
  }

  investMoney() {
    alert('Funcionalidad: Invertir dinero');
  }

  newCollection() {
    alert('Funcionalidad: Nueva cobranza');
  }

  viewCollections() {
    alert('Funcionalidad: Ver cobranzas');
  }

  generateReport() {
    alert('Funcionalidad: Generar reporte');
  }
}