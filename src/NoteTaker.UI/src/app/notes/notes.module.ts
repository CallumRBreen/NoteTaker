import { AppModule } from './../app.module';
import { LimitToPipe } from './../core/pipes/limitTo.pipe';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NotesComponent } from './notes.component';
import { NoteComponent } from './note/note.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MaterialModule } from '../material.module';
import { SharedModule } from '../shared.module';

@NgModule({
  imports: [
    CommonModule,
    FlexLayoutModule,
    MaterialModule,
    SharedModule
  ],
  declarations: [NotesComponent, NoteComponent]
})
export class NotesModule {

}
