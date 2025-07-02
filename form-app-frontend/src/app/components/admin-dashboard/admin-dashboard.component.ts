import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <div class="container mt-5">
      <div class="card">
        <div class="card-header bg-primary text-white">
          <h2>Admin Dashboard</h2>
        </div>
        <div class="card-body">
          <p class="lead">Welcome to the admin dashboard</p>
          <div class="d-grid gap-3">
            <a routerLink="/forms" class="btn btn-primary">Manage Forms</a>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: []
})
export class AdminDashboardComponent {
  constructor() { }
}