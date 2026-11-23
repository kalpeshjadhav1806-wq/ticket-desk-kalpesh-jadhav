import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login.component';
import { TicketListComponent } from './features/tickets/ticket-list/ticket-list.component';
import { CreateTicketComponent } from './features/tickets/create-ticket/create-ticket.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'tickets', component: TicketListComponent },
  { path: 'tickets/create', component: CreateTicketComponent },
  { path: '', redirectTo: 'login', pathMatch: 'full' }
];
