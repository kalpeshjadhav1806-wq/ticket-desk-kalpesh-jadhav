import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { TicketService, Ticket } from '../../../core/services/ticket.service';

@Component({
  selector: 'app-ticket-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './ticket-list.component.html',
  styleUrl: './ticket-list.component.scss'
})
export class TicketListComponent implements OnInit {

  tickets: Ticket[] = [];
  loading = false;
  errorMessage = '';

  constructor(private ticketService: TicketService) { }

  ngOnInit(): void {
    this.loadTickets();
  }

  loadTickets(): void {
    this.loading = true;
    this.errorMessage = '';

    this.ticketService.getMyTickets().subscribe({
      next: (response) => {
        this.tickets = response;
        this.loading = false;
      },
      error: (error) => {
        this.loading = false;
        this.errorMessage = 'Unable to load tickets. Please try again.';
        console.error('Ticket loading failed:', error);
      }
    });
  }

  countByStatus(status: string): number {
    return this.tickets.filter(
      ticket => ticket.status.toLowerCase() === status.toLowerCase()
    ).length;
  }
}
