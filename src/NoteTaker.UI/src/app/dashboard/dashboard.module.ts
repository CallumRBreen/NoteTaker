import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard.component';
import { NoteListComponent } from './note-list/note-list.component';
import { NoteService } from '../core/services/Note.service';

@NgModule({
  imports: [
    CommonModule
  ],
  providers: [NoteService],
  declarations: [DashboardComponent, NoteListComponent],
  exports: [
    DashboardComponent
  ]
})
export class DashboardModule { }
