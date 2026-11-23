import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Ticket {
  id: number;
  title: string;
  description: string;
  priority: string;
  status: string;
  createdAt: string;
  assignedTo: number | null;
}
export interface CreateTicketRequest {
  title: string;
  description: string;
  priority: string;
}

@Injectable({
  providedIn: 'root'
})
export class TicketService {
  private readonly apiUrl = 'https://localhost:7143/api/Tickets';

  constructor(private http: HttpClient) { }

  getMyTickets(): Observable<Ticket[]> {
    return this.http.get<Ticket[]>(`${this.apiUrl}/my-tickets`);
  }
  createTicket(request: CreateTicketRequest): Observable<any> {
    return this.http.post(`${this.apiUrl}`, request);
  }
}
