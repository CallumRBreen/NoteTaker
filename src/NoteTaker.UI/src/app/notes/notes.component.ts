import { DeleteNoteDialogComponent } from './delete-note-dialog/delete-note-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { Component, OnInit } from '@angular/core';
import { NoteService } from '../core/services/note.service';
import { Note } from '../core/models/note';
import { FormControl } from '@angular/forms';
import { Subscription } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-notes',
  templateUrl: './notes.component.html',
  styleUrls: ['./notes.component.css']
})
export class NotesComponent implements OnInit {

  selectedNote: Note;
  notes: Note[] = [];
  noteSearch: string;
  searchNotesControl = new FormControl('');
  searchNotesSubscription = new Subscription();
  orderBy: string;

  constructor(private noteService: NoteService, public dialog: MatDialog) {

  }

  ngOnInit() {
    this.noteService.getNotes().subscribe(notes => {
      this.notes = notes;
      this.selectedNote = notes[0];
    });

    this.searchNotesSubscription = this.searchNotesControl.valueChanges.pipe(debounceTime(500)).subscribe(() => {
      this.noteService.searchNotes(this.searchNotesControl.value, this.orderBy).subscribe(notes => {
        this.notes = notes;
        this.selectedNote = notes[0];
      })
    });
  }

  onSelect(note: Note) {
    this.selectedNote = note;
  }

  openDeletionDialog(id: string, index: number) {
    const dialogRef = this.dialog.open(DeleteNoteDialogComponent, {
      width: '350px',
      data: id
    });

    dialogRef.afterClosed().subscribe(noteToDelete => {
      this.deleteNote(noteToDelete, index);
    });
  }

  ngOnDestroy(): void {
    this.searchNotesSubscription.unsubscribe();
  }

  getNotesOrdered(orderBy: string) {
    this.orderBy = orderBy;
    this.noteService.searchNotes(this.searchNotesControl.value, orderBy).subscribe(notes => {
      this.notes = notes;
      this.selectedNote = notes[0];
    })
  }

  deleteNote(id: string, index: number) {
    this.noteService.deleteNote(id).subscribe(() => {
      this.notes.splice(index, 1);
      this.notes = [...this.notes]
      this.selectedNote = this.notes[0];
    })
  }

  addNote() {
    this.noteService.addNote({ title: '', content: '' }).subscribe(createdNote => {
      this.notes.unshift(createdNote);
      this.notes = [...this.notes]
      this.selectedNote = this.notes[0];
    });
  }
}
