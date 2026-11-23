import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TicketService } from '../../../core/services/ticket.service';

@Component({
  selector: 'app-create-ticket',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './create-ticket.component.html',
  styleUrl: './create-ticket.component.scss'
})
export class CreateTicketComponent {

  loading = false;
  errorMessage = '';

  ticketForm = this.fb.group({
    title: ['', [Validators.required, Validators.maxLength(150)]],
    description: ['', Validators.required],
    priority: ['Medium', Validators.required]
  });

  constructor(
    private fb: FormBuilder,
    private ticketService: TicketService,
    private router: Router
  ) { }

  submitTicket(): void {
    if (this.ticketForm.invalid) {
      this.ticketForm.markAllAsTouched();
      return;
    }

    this.loading = true;
    this.errorMessage = '';

    this.ticketService.createTicket(this.ticketForm.getRawValue() as {
      title: string;
      description: string;
      priority: string;
    }).subscribe({
      next: () => {
        this.router.navigate(['/tickets']);
      },
      error: (error) => {
        this.loading = false;
        this.errorMessage =
          error.error?.message || 'Unable to create ticket. Please try again.';
      }
    });
  }

  cancel(): void {
    this.router.navigate(['/tickets']);
  }
}
