import { Routes } from '@angular/router';
import { FormListComponent } from './components/form-list/form-list.component';
import { FormDetailComponent } from './components/form-detail/form-detail.component';
import { FormEditComponent } from './components/form-edit/form-edit.component';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard.component';

export const routes: Routes = [
  // Redirect root to admin dashboard
  { path: '', redirectTo: '/admin', pathMatch: 'full' },

  // Admin routes only
  { path: 'admin', component: AdminDashboardComponent },
  { path: 'admin/forms', component: FormListComponent },
  { path: 'admin/forms/:id', component: FormDetailComponent },
  { path: 'admin/forms/edit/:id', component: FormEditComponent },

  // Backward compatibility (redirect old routes to admin)
  { path: 'forms', redirectTo: '/admin/forms', pathMatch: 'full' },
  { path: 'forms/:id', redirectTo: '/admin/forms/:id' },
  { path: 'forms/edit/:id', redirectTo: '/admin/forms/edit/:id' },

  // Catch all redirect to admin
  { path: '**', redirectTo: '/admin' }
];
