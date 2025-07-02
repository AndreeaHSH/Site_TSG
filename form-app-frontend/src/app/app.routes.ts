import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { StudentFormComponent } from './components/student-form/student-form.component';
import { FormListComponent } from './components/form-list/form-list.component';
import { FormDetailComponent } from './components/form-detail/form-detail.component';
import { FormEditComponent } from './components/form-edit/form-edit.component';
import { AdminDashboardComponent } from './components/admin-dashboard/admin-dashboard.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'form', component: StudentFormComponent },
  { path: 'forms', component: FormListComponent },
  { path: 'forms/edit/:id', component: FormEditComponent },
  { path: 'forms/:id', component: FormDetailComponent },
  { path: 'admin', component: AdminDashboardComponent },
  { path: '**', redirectTo: '' }
];