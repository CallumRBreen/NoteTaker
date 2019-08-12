import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NotesComponent } from './notes/notes.component';
import { NoteComponent } from './notes/note/note.component';

const routes: Routes = [
  {
    path: '', component: NotesComponent
  },
  {
    path: 'notes', component: NotesComponent, children: [{ path: 'notes/:id', component: NoteComponent }]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
