import { AppRoutingModule } from './../app-routing.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotesComponent } from './notes.component';
import { NoteComponent } from './note/note.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MaterialModule } from '../material.module';
import { SharedModule } from '../shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { DeleteNoteDialogComponent } from './delete-note-dialog/delete-note-dialog.component';



@NgModule({
  imports: [
    CommonModule,
    FlexLayoutModule,
    MaterialModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    CKEditorModule,
    AppRoutingModule
  ],
  declarations: [NotesComponent, NoteComponent, DeleteNoteDialogComponent],
  entryComponents: [DeleteNoteDialogComponent]
})
export class NotesModule {

}
